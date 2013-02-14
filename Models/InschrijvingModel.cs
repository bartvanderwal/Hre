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
using HRE.Business;

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


        public int ParticipationId { get; set; } 

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
        
        [StringLength(100)]
        public string HebJeErZinIn { get; set; }

        [StringLength(255)]
        public string OpmerkingenTbvSpeaker { get; set; }
        
        [StringLength(255)]
        public string Bijzonderheden { get; set; }

        private bool? _isEarlyBird;

        /// <summary>
        /// The user gets the Early Bird discount if he/she was a participant in 2012 and is again in 2013 and is with the first 200.
        /// </summary>
        public bool? IsEarlyBird { 
            get {
                if (!_isEarlyBird.HasValue) {
                        _isEarlyBird = ExternalEventIdentifier==InschrijvingenRepository.H2RE_EVENTNR 
                            && SportsEventParticipationDal.GetByUserIdEventId(UserId, SportsEventDal.Hre2012Id)!=null
                            && LogonUserDal.DetermineNumberOfParticipants(true) < HreSettings.AantalEarlyBirdStartPlekken;
                }
                return _isEarlyBird.Value;
            }
            set {
                _isEarlyBird = value;
            }
        }


        private int? _inschrijfGeld { get; set; }

       
        public int EarlyBirdKorting {
            get {
                return IsEarlyBird.HasValue && IsEarlyBird.Value ? HreSettings.HoogteEarlyBirdKorting : 0;
            }
        }


        /// <summary>
        /// Voor niet NTB leden en leden die GEEN wedstrijd/atleten 'A' licentie hebben zijn er de kosten voor een daglicentie.
        /// </summary>
        public int KostenDagLicentie {
            get {
                if (string.IsNullOrEmpty(LicentieNummer) || (LicentieNummer.Substring(2,1)!="A" && LicentieNummer.Substring(2,1)!="a")) {
                    return HreSettings.KostenDagLicentie;
                } else {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Voor wie geen eigen (MyLaps) chip heeft komt er nog kosten van huur bij.
        /// </summary>
        public int KostenChip {
            get {
                return (string.IsNullOrEmpty(MyLapsChipNummer)) ? HreSettings.KostenHuurMyLapsChipGeel : 0;
            }
        }


        public int? BasisKosten { 
            get {
                return HreSettings.HuidigeDeelnameBedrag;
            }
        }


        public int? InschrijfGeld { 
            get {
                // Voor HRE 2012 geef het uit NTB inschrijvingen eventueel gelezen bedrag terug.
                if (ExternalEventIdentifier==InschrijvingenRepository.HRE_EVENTNR) {
                    return _inschrijfGeld;
                }

                // Voor HRE 2013 wordt het bedrag bepaald afhankelijk van gebruikersgegevens en vaste bedragen in de appsettings.
                // Dit gebeurd dus analoog aan maar apart van berekening in client side JavaScript om 'hackmogelijkheden' uit te sluiten.
                return BasisKosten + KostenDagLicentie + KostenChip - EarlyBirdKorting;
            }

            set {
                _inschrijfGeld = value;
            }
        }


        public bool Food { get; set; }


        public bool Camp { get; set; }


        public bool Bike { get; set; }


        [StringLength(12)]
        public string YouTubeVideoCode { get; set; }

        // Calculated values (only getter, based on above properties). ////////////

        // Calculated values (only getter, based on above properties).
        public string VolledigeNaam {
            get {
                return Voornaam + " " + Tussenvoegsel + " " + Achternaam;
            }
        }


        public string VolledigeNaamMetAanhef {
            get {
                return ((Geslacht=="M") ? "Mr." : "Mevr.") + " " + VolledigeNaam;
            }
        }


        public InschrijvingModel() {
        }


        /// <summary>
        /// Geeft aan of editen toegestaan is of niet. Wordt gebruikt in de GUI.
        /// Editen is alleen toegestaan voor admin gebruikers of als dit de huidige gebruikers eigen inschrijving is.
        /// </summary>
        public bool IsEditAllowed {
            get {
                var currentUser = LogonUserDal.GetCurrentUser();
                return  IsAdmin || (currentUser!=null && currentUser.EmailAddress==Email);
            }
        }



        /// <summary>
        /// Geeft aan of de gebruiker admin rechten heeft.
        /// </summary>
        public bool IsAdmin {
            get {
                return Roles.IsUserInRole("Admin");
            }
        }

        public DateTime DateRegistered { get; set; }


        /// <summary>
        /// The Human Readable/mensvriendelijke versie van de inschrijfdatum.
        /// </summary>
        public string DateRegisteredHR { 
            get {
                return DateRegistered.RelativeDateDescription();
            }
        }

        /// <summary>
        /// Geeft aan of de inschrijving nieuw is.
        /// </summary>
        public bool IsNew { 
            get {
                return (DateRegistered==null || DateRegistered==DateTime.MinValue);
            }
        }        

    }
}
