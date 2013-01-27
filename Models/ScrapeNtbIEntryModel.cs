using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using HRE.Data;
using HRE.Dal;

namespace HRE.Models {

    public class ScrapeNtbIEntryModel : BaseRepository {
        
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

        public DateTime? DateFirstScraped { get; set; }

        public DateTime? DateLastScraped { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        // The external identifier for the entry (ntbI number).
        public string ExternalIdentifier { get; set; }

        // The external identifier for the event (ntbI event number; note that for now assume only 1 serie per event).
        public string ExternalEventIdentifier { get; set; }

        // The external identifier for the event serie (ntbI serie number; note that for now assume only 1 serie per event).
        public string  ExternalEventSerieIdentifier { get; set; }


        [Required(ErrorMessage = "Geef je voornaam op.")]
        [DataType(DataType.Password)]
        [Display(Name = "Voornaam")]
        [StringLength(100, ErrorMessage = "Het {0} moet tenminste {2} karakters lang zijn.", MinimumLength = 6)]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Geef je geboortedatum op.")]
        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        [StringLength(8)]       
        public DateTime? GeboorteDatum { get; set; }

        [StringLength(10)]
        public string Tussenvoegsel { get; set; }

        public string LicentieNummer { get; set; }
        
        [StringLength(10)]
        [Required(ErrorMessage = "Geef je achternaam op.")]
        public string Achternaam { get; set; }

        [StringLength(12)]
        public string ChampionChipNummer { get; set; }

        public string MaatTshirt { get; set; }

        public bool? InteresseNieuwsbrief { get; set; }

        public bool? InteresseOvernachtenNaWedstrijd { get; set; }

        public string Geslacht { get; set; }
        
        [StringLength(30)]
        public string Straat { get; set; }

        public string Huisnummer { get; set; }

        public string HuisnummerToevoeging { get; set; }

        public string Postcode { get; set; }
        
        [StringLength(50)]
        public string Woonplaats { get; set; }

        [StringLength(50)]
        public string Land { get; set; }

        [StringLength(15)]
        public string Telefoon { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string OpmerkingenTbvSpeaker { get; set; }
        
        [StringLength(255)]
        public string Bijzonderheden { get; set; }

        public int? Deelnamebedrag { get; set; }

        public bool BlijftKamperen { get; set; }

        [StringLength(12)]
        public string YouTubeVideoCode { get; set; }

        // Calculated values (only getter, based on above properties).
        public string FullName {
            get {
                return ((Geslacht=="M") ? "Mr." : "Mevr.") + " " + Voornaam + " " + Tussenvoegsel + " " + Achternaam;
            }
        }

        public string DeelnameBedragFormatted {
            get { 
                return Deelnamebedrag.HasValue ? string.Format("&euro; {0}", (((double) Deelnamebedrag.Value)/100).ToString()) : "-";
            }
        }


        public ScrapeNtbIEntryModel() {
        }


        // Construct an ScrapeNtbIEntrymodel from a user.
        public ScrapeNtbIEntryModel(LogonUserDal logonUser) {
            logonuser user = (from logonuser u in DB.logonuser where u.Id==logonUser.ID select u).FirstOrDefault();
            
            ScrapeNtbIRepository.SelectEntries(null, logonUser.ID);
        }


    }
}
