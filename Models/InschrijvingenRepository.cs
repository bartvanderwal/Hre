using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using HRE.Models;
using HRE.Data;
using HRE.Common;
using HRE.Dal;

namespace HRE.Models {


    /// <summary>
    /// Class with utility functions to put into and retrieve Ntb entries from the database.
    /// </summary>
    public class InschrijvingenRepository : BaseRepository {

        #region :: Constants

        public const string HRE_NAME = "HRE";

        public const string HRE_EVENTNR="2005881";

        public const string HRE_SERIENR = "4549";

        public static DateTime HRE_DATE = new DateTime(2012,8,4,16,00,0);

        public const string H2RE_NAME = "H2RE";

        public const string H2RE_EVENTNR="2005971";

        public const string H2RE_SERIENR = "5089";

        public static DateTime H2RE_DATE = new DateTime(2013,8,3,15,30,0);

        public static string ADMIN_ROLE_NAME = "Admin";

        #endregion :: Constants


        /// <summary>
        /// Create the admin role and add all users in the adminUsers (if not yet present) and assign them the admin role (if not yet given).
        /// </summary>
        public static void CreateAdminRoleAndAdmins() {
            if (!Roles.RoleExists(ADMIN_ROLE_NAME)) {
                Roles.CreateRole(ADMIN_ROLE_NAME);
            }

            List<AutoCreatedUser> listOfAdmins = new List<AutoCreatedUser>() { 
                new AutoCreatedUser("bart@hetrondjeeilanden.nl", "24dec2012"), 
                new AutoCreatedUser("yordi@hetrondjeeilanden.nl", "24dec2012"), 
                new AutoCreatedUser("pieter@hetrondjeeilanden.nl", "24dec2012"), 
                new AutoCreatedUser("rudo@hetrondjeeilanden.nl", "24dec2012"),
                new AutoCreatedUser("cock@hetrondjeeilanden.nl", "24dec2012"),
                new AutoCreatedUser("ad@hetrondjeeilanden.nl", "24dec2012"),
                new AutoCreatedUser("kitty@hetrondjeeilanden.nl", "24dec2012")
            };

            List<AutoCreatedUser> listOfTestUsers = new List<AutoCreatedUser>() { 
                new AutoCreatedUser("tester1@hetrondjeeilanden.nl", "24dec2012"), 
                new AutoCreatedUser("tester2@hetrondjeeilanden.nl", "24dec2012"), 
            };

            foreach(AutoCreatedUser adminUser in listOfAdmins) {
                MembershipUser user = Membership.GetUser(adminUser.UserNameAndEmail);
                if (user==null) {
                    LogonUserDal logonUser = LogonUserDal.CreateOrRetrieveUser(adminUser.UserNameAndEmail, adminUser.Password);
                    if (!Roles.IsUserInRole(adminUser.UserNameAndEmail, ADMIN_ROLE_NAME)) {
                        Roles.AddUserToRole(adminUser.UserNameAndEmail, ADMIN_ROLE_NAME); 
                    }
                }
            }


            foreach(AutoCreatedUser testUser in listOfTestUsers) {
                MembershipUser user = Membership.GetUser(testUser.UserNameAndEmail);
                if (user==null) {
                    LogonUserDal logonUser = LogonUserDal.CreateOrRetrieveUser(testUser.UserNameAndEmail, testUser.Password);
                }
            }


            LogonUserDal bart = LogonUserDal.GetByEmailAddress("bart@hetrondjeeilanden.nl");
            if (bart!=null && !bart.DateOfBirth.HasValue) {
                bart.DateCreated = DateTime.Now;
                bart.DateOfBirth = DateTime.ParseExact("27/06/1977", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                bart.IsMailingListMember = true;
                bart.IsActive = true;
                bart.Gender = true;
                bart.UserName = bart.EmailAddress;
                bart.Save();
            }
        }


        public static sportsevent GetHreEvent() {
            return GetOrCreateEvent(HRE_EVENTNR, HRE_SERIENR, HRE_NAME, HRE_DATE);
        }

        
        public static sportsevent GetH2reEvent() {
            return GetOrCreateEvent(H2RE_EVENTNR, HRE_SERIENR, H2RE_NAME, H2RE_DATE);
        }


        /// <summary>
        /// Get an event by it's external Id (=NTB inschrijvingen evenement nummer).
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public static InschrijvingModel GetByExternalIdentifier(string externalId, string eventNr) {
            var currentEntry = (from entry in SelectEntries(eventNr)
                                where entry.ExternalIdentifier==externalId
                                select entry
                ).FirstOrDefault();
            return currentEntry;
        }

        /// <summary>
        /// Get entries.
        /// </summary>
        /// <param name="eventNr"></param>
        /// <returns></returns>
        public static List<InschrijvingModel> GetEntries(string eventNr) {
            return SelectEntries(eventNr).ToList();
        }


        /// <summary>
        /// Get entries, by mapping information from user databables (eventparticipation, logonuser and address) to an NTB inschrijvingen entry.
        /// </summary>
        /// <param name="eventNr"></param>
        /// <returns></returns>
        public static IEnumerable<InschrijvingModel> SelectEntries(string eventNr, int userId = 0) {
            hreEntities DB = DBConnection.GetHreContext();
            
            var raceEntries = from sportseventparticipation p in DB.sportseventparticipation
                join sportsevent e in DB.sportsevent on p.SportsEventId equals e.Id
                join logonuser u in DB.logonuser on p.UserId equals u.Id 
                join address a in DB.address on u.PrimaryAddressId equals a.Id
                where e.ExternalEventIdentifier == eventNr
                select new InschrijvingModel() {
                    // User data.    
                    UserId = u.Id,
                    UserName = u.UserName,
                    GeboorteDatum = u.DateOfBirth,
                    Geslacht = u.Gender.HasValue ? (u.Gender.Value ? "M" : "V") : string.Empty,
                    LicentieNummer = u.NtbLicenseNumber,
                    Email = u.EmailAddress,
                    Telefoon = u.TelephoneNumber,

                    // Address data.
                    Voornaam = a.Firstname,
                    Tussenvoegsel = a.Insertion,
                    Achternaam = a.Lastname,
                    InteresseNieuwsbrief = u.IsMailingListMember,
                    Straat = a.Street,
                    Huisnummer = a.Housenumber,
                    HuisnummerToevoeging = a.HouseNumberAddition,
                    Postcode = a.PostalCode,
                    Woonplaats = a.City,
                    Land = a.Country,

                    // Sportsevent data.
                    ExternalEventIdentifier = e.ExternalEventIdentifier,
                    ExternalEventSerieIdentifier = e.ExternalEventSerieIdentifier,
                
                    // Sportseventparticipation data.
                    ExternalIdentifier = p.ExternalIdentifier,
                    RegistrationDate = p.DateRegistered,
                    
                    // TODO BW 2013-02-06: Rename the database fields from 'Scraped' also to 'Synchronized' like the ORM fields
                    // once this is an accurate description (e.g. when posting 'updates' and 'inserts' to NTB inschrijvingen is also done on Save).
                    DateFirstSynchronized = p.DateFirstScraped,
                    DateLastSynchronized = p.DateLastScraped,
                    // END TODO

                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated,
                    MyLapsChipNummer = p.MyLapsChipIdentifier,
                    MaatTshirt = p.TShirtSize,
                    InteresseOvernachtenNaWedstrijd = p.IsInterestedToSleepOver,
                    OpmerkingenTbvSpeaker = p.SpeakerRemarks,
                    Bijzonderheden = p.Notes,
                    
                    IsEarlyBird = p.EarlyBird.HasValue && p.EarlyBird.Value
                    
                };

            if (userId!=0) {
                raceEntries = raceEntries.Where(e => e.UserId==userId);
            }

            return raceEntries;
        }


        /// <summary>
        /// Gets an event by external=eventNumber, or creates it in the database if it doesn't exist yet.
        /// </summary>
        /// <param name="eventNumber"></param>
        /// <param name="eventName"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        private static sportsevent GetOrCreateEvent(string eventNumber, string serieNumber, string eventName, DateTime eventDate) {
            sportsevent hreEvent = GetEvent(eventNumber);
            if (hreEvent==null) {
                hreEvent = new sportsevent();
                hreEvent.Name=HRE_NAME;
                hreEvent.ExternalEventIdentifier=eventNumber;
                hreEvent.ExternalEventSerieIdentifier=serieNumber;
                hreEvent.DateCreated = DateTime.Now;
                hreEvent.DateUpdated = DateTime.Now;
                hreEvent.EventDate = eventDate;
                hreEvent.EventPlace = "Vinkeveen - Zandeiland 1";
                DB.AddTosportsevent(hreEvent);
                DB.SaveChanges();
            }
            return hreEvent;
        }


        /// <summary>
        /// Gets an event by external=eventNumber, or creates it in the database if it doesn't exist yet.
        /// </summary>
        /// <param name="eventNumber"></param>
        public static sportsevent GetEvent(string eventNumber) {
            return (from e in DB.sportsevent where e.ExternalEventIdentifier==eventNumber select e).FirstOrDefault();
        }


        /// <summary>
        /// Sav an entry.
        /// </summary>
        /// <param name="inschrijving"></param>
        /// <param name="eventNumber"></param>
        public static void SaveEntry(InschrijvingModel inschrijving, string eventNumber, bool isScrape, bool overrideLocallyUpdated = false) {

            // TODO BW 2013-02-6 Als een inschrijving wordt gesaved die is gescraped van NTB inschrijvingen, maar het e-mail adres is
            // inmiddels aangepast dan werkt onderstaande niet. De persoon wordt dan onder een nieuwe inschrijving gesaved en klapt er mogelijk uit op duplicate NTB licentie nummer..

            // Create and/or retrieve the user (and the underlying ASP.NET membership user).
            LogonUserDal user = LogonUserDal.CreateOrRetrieveUser(inschrijving.Email, "", inschrijving.ExternalIdentifier);

            // Update the rest of the user data ONLY if:
            // - It is NOT being scraped for this save
            // - Or the raceEntry is first created
            // - OR it IS being scraped but was scraped before and was NOT updated after it was last scraped (e.g. DateUpdated<=DateLastScraped)
            // TODO: Update (and save) only if the information is newer; somehow...
            if (overrideLocallyUpdated || !isScrape || !user.DateOfBirth.HasValue || (inschrijving.DateLastSynchronized.HasValue 
                                && DateTime.Compare(inschrijving.DateUpdated, inschrijving.DateLastSynchronized.Value)<=0)) {
                user.DateOfBirth = inschrijving.GeboorteDatum;
                user.IsMailingListMember = inschrijving.InteresseNieuwsbrief;
                user.UserName = inschrijving.Email;
                user.EmailAddress = inschrijving.Email;
                user.TelephoneNumber = inschrijving.Telefoon;

                user.NtbLicenseNumber = inschrijving.HasLicentieNummer ? inschrijving.LicentieNummer : null;
                
                user.IsActive = false;
                if (!string.IsNullOrEmpty(inschrijving.Geslacht)) {
                    user.Gender = inschrijving.Geslacht=="M";
                }

                // If the user has no address or the addres was updated later then the user then add an address.
                address address = user.PrimaryAddress;
                if (address==null) {
                    throw new ArgumentException("address is null. This should not occur. This code should be able to be deleted :P.");
                    // Create an address.
                    // address = new address();
                    // address.DateCreated = DateTime.Now;
                }
                address.DateUpdated = DateTime.Now;
                address.City = inschrijving.Woonplaats;
                address.Street = inschrijving.Straat;
                address.Housenumber = inschrijving.Huisnummer;
                address.HouseNumberAddition = inschrijving.HuisnummerToevoeging;
                address.Country = inschrijving.Land;
                address.Firstname = inschrijving.Voornaam;
                address.Insertion = inschrijving.Tussenvoegsel;
                address.Lastname = inschrijving.Achternaam;
                address.PostalCode = inschrijving.Postcode;
                // DB.SaveChanges();
                user.PrimaryAddress = address; 
                user.Save();

                int eventId = InschrijvingenRepository.GetEvent(eventNumber).Id;

                // Create the sportsparticipation.
                sportseventparticipation participation = (from p in DB.sportseventparticipation where p.UserId==user.Id && p.SportsEventId ==  eventId select p).FirstOrDefault();

                if (participation==null) {
                    participation = new sportseventparticipation();
                    participation.DateCreated=DateTime.Now;
                    SportsEventDal sportsevent = SportsEventDal.GetByExternalId(eventNumber);
                    participation.SportsEventId = sportsevent.ID;
                }

                if (isScrape) {
                    if (!participation.DateFirstScraped.HasValue) {
                        participation.DateFirstScraped = DateTime.Now;
                    }
                    participation.DateLastScraped = DateTime.Now;
                    participation.DateUpdated=participation.DateLastScraped.Value;
                } else {
                    participation.DateUpdated=DateTime.Now;
                    participation.DateRegistered = participation.DateUpdated;
                }
                participation.DateRegistered=inschrijving.RegistrationDate;
                if (isScrape) {
                    participation.ExternalIdentifier=inschrijving.ExternalIdentifier;
                }
                participation.SportsEventId = (from sportsevent se in DB.sportsevent 
                                                where se.ExternalEventIdentifier==eventNumber 
                                                select se.Id).First();
                participation.UserId = user.Id;

                participation.SpeakerRemarks = inschrijving.OpmerkingenTbvSpeaker;
                participation.IsInterestedToSleepOver = inschrijving.InteresseOvernachtenNaWedstrijd;
                participation.TShirtSize = inschrijving.MaatTshirt;
                participation.ParticipationStatus = 1;
                participation.MyLapsChipIdentifier = inschrijving.HasMyLapsChipNummer ? inschrijving.MyLapsChipNummer : string.Empty;
                participation.Notes = inschrijving.Bijzonderheden;

                participation.ParticipationAmountInEuroCents = inschrijving.InschrijfGeld;

                if (participation.Id==0) {
                    DB.AddTosportseventparticipation(participation);
                }
                DB.SaveChanges();
            }
        }



        public class AutoCreatedUser {

            public string UserNameAndEmail { get; private set; } 
            
            public string Password { get; private set; }

            public AutoCreatedUser(string userNameAndEmail, string password) {
                UserNameAndEmail = userNameAndEmail;
                Password = password;
            }
        }


    }
}
