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
    public class ScrapeNtbIRepository : BaseRepository {

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

            List<AdminUser> listOfAdmins = new List<AdminUser>() { 
                new AdminUser("bart@hetrondjeeilanden.nl", "24dec2012"), 
                new AdminUser("yordi@hetrondjeeilanden.nl", "24dec2012"), 
                new AdminUser("pieter@hetrondjeeilanden.nl", "24dec2012"), 
                new AdminUser("rudo@hetrondjeeilanden.nl", "24dec2012"),
                new AdminUser("cock@hetrondjeeilanden.nl", "24dec2012"),
                new AdminUser("ad@hetrondjeeilanden.nl", "24dec2012"),
                new AdminUser("kitty@hetrondjeeilanden.nl", "24dec2012")
            };

            foreach(AdminUser adminUser in listOfAdmins) {
                MembershipUser user = Membership.GetUser(adminUser.UserNameAndEmail);
                if (user==null) {
                    LogonUserDal logonUser = LogonUserDal.CreateOrRetrieveUser(adminUser.UserNameAndEmail, adminUser.Password);
                    if (!Roles.IsUserInRole(adminUser.UserNameAndEmail, ADMIN_ROLE_NAME)) {
                        Roles.AddUserToRole(adminUser.UserNameAndEmail, ADMIN_ROLE_NAME); 
                    }
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
        public static ScrapeNtbIEntryModel GetByExternalIdentifier(string externalId, string eventNr = "") {
            if (string.IsNullOrEmpty(eventNr)) {
                eventNr = HRE_EVENTNR;
            }
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
        public static List<ScrapeNtbIEntryModel> GetEntries(string eventNr) {
            return SelectEntries(eventNr).ToList();
        }


        /// <summary>
        /// Get entries, by mapping information from user databables (eventparticipation, logonuser and address) to an NTB inschrijvingen entry.
        /// </summary>
        /// <param name="eventNr"></param>
        /// <returns></returns>
        public static IEnumerable<ScrapeNtbIEntryModel> SelectEntries(string eventNr, int userId = 0) {
            hreEntities DB = DBConnection.GetHreContext();
            
            var raceEntries = from sportseventparticipation p in DB.sportseventparticipation
                join sportsevent e in DB.sportsevent on p.SportsEventId equals e.Id
                join logonuser u in DB.logonuser on p.UserId equals u.Id 
                join address a in DB.address on u.PrimaryAddressId equals a.Id
                where e.ExternalEventIdentifier == eventNr
                select new ScrapeNtbIEntryModel() {
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
                    DateFirstScraped = p.DateFirstScraped,
                    DateLastScraped = p.DateLastScraped,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated,
                    ChampionChipNummer = p.MyLapsChipIdentifier,
                    MaatTshirt = p.TShirtSize,
                    InteresseOvernachtenNaWedstrijd = p.IsInterestedToSleepOver,
                    OpmerkingenTbvSpeaker = p.SpeakerRemarks,
                    Bijzonderheden = p.Notes,
                    Deelnamebedrag = p.ParticipationAmountInEuroCents
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
            sportsevent hreEvent = (from e in DB.sportsevent where e.ExternalEventIdentifier==eventNumber select e).FirstOrDefault();
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
        /// Sav an entry.
        /// </summary>
        /// <param name="raceEntry"></param>
        /// <param name="eventNumber"></param>
        public static void SaveEntry(ScrapeNtbIEntryModel raceEntry, string eventNumber, bool isScrape) {

            // Create and/or retrieve the user (and the underlying ASP.NET membership user).
            LogonUserDal user = LogonUserDal.CreateOrRetrieveUser(raceEntry.Email);

            // Update the rest of the user data ONLY if:
            // - It is NOT being scraped for this save
            // - Or the raceEntry is first created
            // - OR it IS being scraped but was scraped before and was NOT updated after it was last scraped (e.g. DateUpdated<=DateLastScraped)
            // TODO: Update (and save) only if the information is newer; somehow...
            if (!isScrape || !user.DateOfBirth.HasValue || (raceEntry.DateLastScraped.HasValue 
                                && DateTime.Compare(raceEntry.DateUpdated, raceEntry.DateLastScraped.Value)<=0)) {
                user.DateOfBirth = raceEntry.GeboorteDatum;
                user.IsMailingListMember = raceEntry.InteresseNieuwsbrief;
                user.UserName = raceEntry.Email;
                user.EmailAddress = raceEntry.Email;
                user.TelephoneNumber = raceEntry.Telefoon;
                string tussenvoegsel = string.IsNullOrEmpty(raceEntry.Tussenvoegsel) ? " " : " " + raceEntry.Tussenvoegsel + " ";
                user.UserName = raceEntry.Voornaam + tussenvoegsel + raceEntry.Achternaam;
                if (!string.IsNullOrEmpty(raceEntry.LicentieNummer)) {
                    user.NtbLicenseNumber = raceEntry.LicentieNummer;
                }
                user.IsActive = false;
                if (!string.IsNullOrEmpty(raceEntry.Geslacht)) {
                    user.Gender = raceEntry.Geslacht=="M";
                }

                // If the user has no address or the addres was updated later then the user then add an address.
                address address = user.PrimaryAddress;
                if (address==null) {
                    // Create an address.
                    address = new address();
                    address.DateCreated = DateTime.Now;
                }
                address.DateUpdated = DateTime.Now;
                address.City = raceEntry.Woonplaats;
                address.Street = raceEntry.Straat;
                address.Housenumber = raceEntry.Huisnummer;
                address.HouseNumberAddition = raceEntry.HuisnummerToevoeging;
                address.Country = raceEntry.Land;
                address.Firstname = raceEntry.Voornaam;
                address.Insertion = raceEntry.Tussenvoegsel;
                address.Lastname = raceEntry.Achternaam;
                address.PostalCode = raceEntry.Postcode;
                DB.SaveChanges();
                user.PrimaryAddress = address; 
                user.Save();

                // TODO BW 2012-12-24: Allow the user to set the current event ID from the interface (right now it's static always 2012).
                int eventId = GetHreEvent().Id;

                // Create the sportsparticipation.
                sportseventparticipation participation = (from p in DB.sportseventparticipation where p.UserId==user.ID && p.ExternalIdentifier == raceEntry.ExternalIdentifier select p).FirstOrDefault();

                if (participation==null) {
                    participation = new sportseventparticipation();
                    participation.DateCreated=DateTime.Now;
                }

                if (isScrape) {
                    if (!participation.DateFirstScraped.HasValue) {
                        participation.DateFirstScraped = DateTime.Now;
                    }
                    participation.DateLastScraped = DateTime.Now;
                    participation.DateUpdated=participation.DateLastScraped.Value;
                } else {
                    participation.DateUpdated=DateTime.Now;
                }
                participation.DateRegistered=raceEntry.RegistrationDate;
                participation.ExternalIdentifier=raceEntry.ExternalIdentifier;
                participation.SportsEventId = eventId;
                participation.UserId = user.ID;
                    
                participation.SpeakerRemarks = raceEntry.OpmerkingenTbvSpeaker;
                participation.IsInterestedToSleepOver = raceEntry.InteresseOvernachtenNaWedstrijd;
                participation.TShirtSize = raceEntry.MaatTshirt;
                participation.ParticipationStatus = 1;
                participation.MyLapsChipIdentifier = raceEntry.ChampionChipNummer;
                participation.Notes = raceEntry.Bijzonderheden;

                if (participation.Id==0) {
                    DB.AddTosportseventparticipation(participation);
                }
                DB.SaveChanges();
            }
        }



        public class AdminUser {

            public string UserNameAndEmail { get; private set; } 
            
            public string Password { get; private set; }

            public AdminUser(string userNameAndEmail, string password) {
                UserNameAndEmail = userNameAndEmail;
                Password = password;
            }
        }


    }
}
