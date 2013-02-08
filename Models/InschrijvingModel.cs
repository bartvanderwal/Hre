using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using HRE.Data;
using HRE.Dal;
using HRE.Common;

namespace HRE.Models {

    public class InschrijvingModel : BaseRepository {
        
        /// <summary>
        /// The user id.
        /// Warning; do NOT set the username from the User form, only initally on scraping/creation.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user name.
        /// Warning; do NOT set the username from the User form, only initally on scraping/creation.
        /// </summary>
        public string UserName { get; set; }
 
        public DateTime RegistrationDate { get; set; }

        public DateTime? DateFirstSynchronized { get; set; }

        public DateTime? DateLastSynchronized { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        // The external identifier for the entry (ntbI number).
        public string ExternalIdentifier { get; set; }

        // The external identifier for the event (ntbI event number; note that for now assume only 1 serie per event).
        public string ExternalEventIdentifier { get; set; }

        // The external identifier for the event serie (ntbI serie number; note that for now assume only 1 serie per event).
        public string  ExternalEventSerieIdentifier { get; set; }


        [Required(ErrorMessage = "Geef je voornaam aan")]
        [Display(Name = "Voornaam")]
        [StringLength(20, ErrorMessage = "De naam {0} mag niet meer dan 20 karakters lang zijn.")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Geef je geboortedatum")]
        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        // [StringLength(8)]
        public DateTime? GeboorteDatum { get; set; }

        [StringLength(10)]
        public string Tussenvoegsel { get; set; }

        private bool? _hasLicentieNummer;
        
        public bool HasLicentieNummer {
            get {
                if (!_hasLicentieNummer.HasValue) {
                    return !string.IsNullOrEmpty(LicentieNummer);
                }
                return _hasLicentieNummer.Value;
            }
            set {
                _hasLicentieNummer = value;
            }
        }
        
        public string LicentieNummer { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Geef je achternaam aan")]
        public string Achternaam { get; set; }

        string _myLapsChipNummer;

        [StringLength(12)]
        public string MyLapsChipNummer {
            get {
                if (string.IsNullOrEmpty(_myLapsChipNummer)) {
                    return string.Empty;
                }
                int indexOfSpace = _myLapsChipNummer.Contains(' ') ? _myLapsChipNummer.IndexOf(' ') : -1;
                // Cut off any possible postfix from the MYLapsChipNumber as read in from NTB inschrijvigen.
                // like for instance the ' Chiptype: geel' in 'DK973S2 Chiptype: geel'
                // (NB We WILL leave it in our database for now, just not for 2013).
                if (indexOfSpace!=-1) {
                    return _myLapsChipNummer.Substring(0, indexOfSpace);
                } else {
                    return _myLapsChipNummer;
                }
            }
            set {
                _myLapsChipNummer = value;
            }
        }

        private bool? _hasMyLapsChipNummer;

        public bool HasMyLapsChipNummer {
            get {
                if (!_hasMyLapsChipNummer.HasValue) {
                    return !string.IsNullOrEmpty(MyLapsChipNummer);
                }
                return _hasMyLapsChipNummer.Value;
            }
            set {
                _hasMyLapsChipNummer = value;
            }
        }

        public string MaatTshirt { get; set; }

        public bool? InteresseNieuwsbrief { get; set; }

        public bool? InteresseOvernachtenNaWedstrijd { get; set; }

        [Required(ErrorMessage = "Geef je geslacht aan")]
        public string Geslacht { get; set; }
        
        [Required(ErrorMessage = "Geef je straat aan")]
        [StringLength(30)]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Geef je huisnummer aan")]
        public string Huisnummer { get; set; }

        public string HuisnummerToevoeging { get; set; }

        [Required(ErrorMessage = "Geef je postcode")]
        public string Postcode { get; set; }
        
        [StringLength(50)]
        [Required(ErrorMessage = "Geef je woonplaats")]
        public string Woonplaats { get; set; }

        [StringLength(50)]
        public string Land { get; set; }

        [StringLength(15)]
        public string Telefoon { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Geef je e-mail adres")]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string OpmerkingenTbvSpeaker { get; set; }
        
        [StringLength(255)]
        public string Bijzonderheden { get; set; }

        public bool IsEarlyBird { get; set; }

        private int? _inschrijfGeld { get; set; }

        public int? InschrijfGeld { 
            get {
                // Voor HRE 2012 geef het uit NTB inschrijvingen eventueel gelezen bedrag terug.
                if (ExternalEventIdentifier==InschrijvingenRepository.GetHreEvent().ExternalEventIdentifier) {
                    return _inschrijfGeld;
                }

                // Voor HRE 2012 bepaal het bedrag afhankelijk van gebruikersgegevens en vaste bedragen in de appsettings.
                // Bepaal de kosten. Dit wordt hier geheel herberekend - idem als in client side JavaScript - om 'hackmogelijkheden' uit te sluiten.
                int bedrag = HreSettings.HuidigeDeelnameBedrag;
                
                // Early birds krijgen korting.
                if (IsEarlyBird) {
                    bedrag = Math.Max(0, bedrag-HreSettings.HoogteEarlyBirdKorting);
                }

                // Voor niet NTB leden en leden die GEEN wedstrijd 'W' licentie hebben komt er de kosten voor daglicentie bij:
                if (string.IsNullOrEmpty(LicentieNummer) || LicentieNummer.Substring(2,1)!="A") {
                    bedrag += HreSettings.KostenDagLicentie;
                }

                // Voor wie geen eigen MyLaps chip heeft komt er nog kosten van huur bij:
                if (string.IsNullOrEmpty(MyLapsChipNummer)) {
                    bedrag += HreSettings.KostenHuurMyLapsChipGeel;
                }

                return bedrag;
            }

            set {
                if (ExternalEventIdentifier==InschrijvingenRepository.GetHreEvent().ExternalEventIdentifier) {
                    _inschrijfGeld = value;
                } else {
                    throw new ArgumentException("Het inschrijfgeld kan niet ingesteld worden, maar wordt binnen Model berekend.");
                }
            }
        }

        public bool BlijftKamperen { get; set; }

        [StringLength(12)]
        public string YouTubeVideoCode { get; set; }

        // Calculated values (only getter, based on above properties).
        public string FullName {
            get {
                return ((Geslacht=="M") ? "Mr." : "Mevr.") + " " + Voornaam + " " + Tussenvoegsel + " " + Achternaam;
            }
        }

        public string InschrijfGeldFormatted {
            get { 
                return InschrijfGeld.HasValue ? string.Format("&euro; {0}", (((double) InschrijfGeld.Value)/100).ToString()) : "-";
            }
        }


        public InschrijvingModel() {
        }


        // Construct an InschrijvingModel from a user.
        public InschrijvingModel(LogonUserDal logonUser) {
            logonuser user = (from logonuser u in DB.logonuser where u.Id==logonUser.Id select u).FirstOrDefault();
            
            InschrijvingenRepository.SelectEntries(null, logonUser.Id);
        }

        /// <summary>
        /// Geeft aan of editen toegestaan is of niet. Wordt gebruikt in de GUI.
        /// Editen is alleen toegestaan voor admin gebruikers of als dit de huidige gebruikers eigen inschrijving is.
        /// </summary>
        public bool IsEditAllowed {
            get {
                var currentUser = LogonUserDal.GetCurrentUser();
                return  Roles.IsUserInRole("Admin") || (currentUser!=null && currentUser.EmailAddress==Email);
            }
        }

    }
}
