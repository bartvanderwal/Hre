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
using HRE.Attributes;
using System.Web.UI.WebControls;
using System.Web;

namespace HRE.Models {

    public class InschrijvingModel : BaseRepository {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public InschrijvingModel() { 
            // Set EmailBeforeUpdateIfAny to value of Email on initialisation, to store old value to be able to determine if it changed..
            EmailBeforeUpdateIfAny = Email;
        }

        public int? StartNummer { get; set; }


        public TimeSpan? StartTijd { get; set; }

        /// <summary>
        /// The User id.
        /// Warning; do NOT set the UserId from the User form, only initally on scraping/creation.
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// The user name.
        /// Warning; do NOT set the UserName from the User form, only initally on scraping/creation.
        /// </summary>
        public string UserName { get; set; }
 
        
        /// <summary>
        /// Geeft aan of de inschrijving voor een nieuwe gebruiker is.
        /// </summary>
        public bool IsNewUser {
            get {
                return UserId==0;
            }
        }


        public LogonUserDal User {
            get {
                return LogonUserDal.GetByID(UserId);
            }
        }

        
        
        public DateTime RegistrationDate { get; set; }

        public DateTime? VirtualRegistrationDateForOrdering { get; set; }
        
        public DateTime? DateFirstSynchronized { get; set; }

        public DateTime? DateLastSynchronized { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        private string _externalIdentifier;

        private static string HRE_EXT_ID_PREFIX = "HRE";

        
        /// <summary>
        /// The external identifier is the one from NTB inschrijvingen or 'HRE+ParticipationId' (not stored but calculated, so this can be changed later).
        /// </summary>
        public string ExternalIdentifier { 
            get {
                if (string.IsNullOrEmpty(_externalIdentifier)) {
                    return  HRE_EXT_ID_PREFIX + ParticipationId;
                } else {
                    return _externalIdentifier; 
                }
            }
            set {
                // Set the value, but prevent if it is a computed value.
                if (!string.IsNullOrEmpty(value) && !value.StartsWith(HRE_EXT_ID_PREFIX)) {
                    _externalIdentifier = value;
                }
            }
        }

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

        [Required(ErrorMessage = "Geef je geslacht aan")]
        public string Geslacht { get; set; }
        
        [Required(ErrorMessage = "Geef je straat aan")]
        [StringLength(30)]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Geef je huisnummer aan")]
        public string Huisnummer { get; set; }

        public string HuisnummerToevoeging { get; set; }

        [Required(ErrorMessage = "Geef je postcode op")]
        public string Postcode { get; set; }
        
        [StringLength(50)]
        [Required(ErrorMessage = "Geef je woonplaats aan")]
        public string Woonplaats { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Selecteer een land")]
        public string Land { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Geef je telefoonnummer op")]
        public string Telefoon { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Geef je e-mail adres op")]
        public string Email { get; set; }
        
        [StringLength(50)]
        public string EmailBeforeUpdateIfAny { get; set; }
        
        [StringLength(100)]
        public string HebJeErZinIn { get; set; }

        [StringLength(255)]
        public string OpmerkingenTbvSpeaker { get; set; }
        
        [StringLength(255)]
        public string OpmerkingenAanOrganisatie { get; set; }

        private bool? _isEarlyBird;

        public DateTime? DateConfirmationSend { get; set; }

        /// <summary>
        /// The user gets the Early Bird discount if he/she was a participant in 2012 and is again in 2013 and is with the first 200 and is still on time for the discount.
        /// </summary>
        public bool? IsEarlyBird { 
            get {
                if (!_isEarlyBird.HasValue) {
                        _isEarlyBird = ExternalEventIdentifier==InschrijvingenRepository.H2RE_EVENTNR 
                            && SportsEventParticipationDal.GetByUserIdEventId(UserId, SportsEventDal.Hre2012Id)!=null
                            && LogonUserDal.AantalIngeschrevenEarlyBirds() < HreSettings.AantalEarlyBirdStartPlekken
                            && DateTime.Compare(DateTime.Now, HreSettings.EindDatumEarlyBirdKorting)<=0;
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
        public int KostenNtbDagLicentie {
            get {
                if (string.IsNullOrEmpty(LicentieNummer) || 
                    (LicentieNummer.Substring(2,1).ToUpper()!="A" && LicentieNummer.Substring(2,1).ToUpper()!="X")) {
                    return HreSettings.KostenNtbDagLicentie;
                } else {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Wie mee wil eten, betaald voor eten :).
        /// </summary>
        public int KostenEten {
            get {
                return Food ? HreSettings.KostenEten : 0;
            }
        }


        /// <summary>
        /// Voor wie geen eigen (MyLaps) chip heeft komen er nog kosten van huur bij.
        /// </summary>
        public int KostenChip {
            get {
                return (!HasMyLapsChipNummer || string.IsNullOrEmpty(MyLapsChipNummer)) ? HreSettings.KostenHuurMyLapsChipGeel : 0;
            }
        }


        public bool? FreeStarter { get; set; }


        public int? BasisKosten { 
            get {
                return (FreeStarter.HasValue && FreeStarter.Value) ? 0 : HreSettings.HuidigeDeelnameBedrag;
            }
        }


        public int? InschrijfGeld { 
            get {
                // Voor HRE 2012 of latere jaren als bedrag al ingevuld is, geef het bedrag uit database terug.
                if (ExternalEventIdentifier==InschrijvingenRepository.HRE_EVENTNR || _inschrijfGeld.HasValue) {
                    return _inschrijfGeld;
                }

                // Voor HRE 2013 wordt het bedrag bepaald afhankelijk van gebruikersgegevens en vaste bedragen in de appsettings.
                // Dit gebeurd dus analoog aan maar apart van berekening in client side JavaScript om 'hackmogelijkheden' uit te sluiten.
                return BasisKosten + KostenNtbDagLicentie + KostenChip + KostenEten - EarlyBirdKorting;
            }
            set {
                _inschrijfGeld = value;
            }
        }

        
        /// <summary>
        /// Het door de deelnemer betaald bedrag.
        /// </summary>
        public int? BedragBetaald { get; set; }

        public DateTime? DatumBetaald { get; set; }

        public bool? IsBetaald { get; set; }

        public bool GenoegBetaaldVoorDeelnemerslijst { get; set; }


        [IsTrue(ErrorMessage = "Meld je aan voor de Flessenpost (e-mail nieuwsbrief voor deelnemers)")]
        /// <summary>
        /// Is de deelnemer aangemeld voor de periodieke e-mail nieuwsbrief (Flessenpost) (true) of niet (false)?
        /// </summary>
        public bool Newsletter { get; set; }

        
        /// <summary>
        /// Wil de deelnemer na afloop mee eten (true) of niet (false)?
        /// </summary>
        public bool Food { get; set; }

        
        /// <summary>
        /// Wil de deelnemer na afloop blijven kamperen (true) of niet (false)?
        /// </summary>
        public bool Camp { get; set; }


        /// <summary>
        /// Wil de deelnemer op de fiets naar het evenement komen (true) of niet (false)?
        /// </summary>
        public bool Bike { get; set; }

        
        /// <summary>
        /// Denkt de deelnemer mee te kunnen doen aan de finale?
        /// </summary>
        public string Finale { get; set; }


        [StringLength(12)]
        public string YouTubeVideoCode { get; set; }

        // Calculated values (only getter, based on above properties). ////////////

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


        /// <summary>
        /// Geeft aan of editen toegestaan is of niet. Wordt gebruikt in de GUI.
        /// Editen is alleen toegestaan voor volledige nieuwe inschrijvingen, voor admin gebruikers of als dit de huidige gebruikers eigen inschrijving is.
        /// </summary>
        public bool IsEditAllowed {
            get {
                var currentUser = LogonUserDal.GetCurrentUser();
                return IsNewUser || IsAdmin || (currentUser!=null && currentUser.EmailAddress==Email);
            }
        }


        /// <summary>
        /// Geeft aan of de gebruiker admin rechten heeft.
        /// </summary>
        public bool IsAdmin {
            get {
                return Roles.IsUserInRole(InschrijvingenRepository.ADMIN_ROLE_NAME);
            }
        }


        /// <summary>
        /// Geeft aan of de gebruiker speaker rechten heeft.
        /// </summary>
        public bool IsSpeaker {
            get {
                return Roles.IsUserInRole(InschrijvingenRepository.SPEAKER_ROLE_NAME);
            }
        }

        /// <summary>
        /// Geeft aan of de inschrijving al geregistreerd is (de gebruiker is bekend/opgeslagen en de inschrijving heeft al een registratiedatum).
        /// </summary>
        public bool IsRegistered { 
            get {
                return !IsNewUser && RegistrationDate!=null && RegistrationDate!=DateTime.MinValue;
            }
        }

        /// <summary>
        /// Indicates whether - for already registered entries - the e-mail confirmation should be resent.
        /// </summary>
        [Display(Name = "Bevestigingsmail van wijziging")]
        public bool DoForceSendConfirmationOfChange { get; set; }

        /// <summary>
        /// This field is NOT stored in the database, but is just to indicate to the (MijnRondjeEilanden) view whether the
        /// logonuser with this subscription has just confirmed his e-mail address or not.
        /// </summary>
        public bool EmailAddressJustConfirmed { get; private set; }


        /// <summary>
        /// Geeft aan of het een inschrijving is van een nieuwe gebruiker (view IkDoeMeeof) of niet (view Edit).
        /// </summary>
        public bool IsInschrijvingNieuweGebruiker { get; set; }


        /// <summary>
        /// Geeft aan of er volledig en correct betaald is. 
        /// </summary>
        public bool IsVolledigEnCorrectBetaald {
            get {
                return IsBetaald.HasValue && IsBetaald.Value; // InschrijfGeld.HasValue && BedragBetaald.HasValue && InschrijfGeld.Value==BedragBetaald.Value && DatumBetaald.HasValue;
            }
        }


        /// <summary>
        /// Geeft aan of er voldoende betaald is om in ieder geval in de inschrijflijst te kunnen staan. 
        /// </summary>
        public bool IsVoldoendeBetaaldVoorStartlijst {
            get {
                return (FreeStarter.HasValue && FreeStarter.Value || (InschrijfGeld.HasValue && BedragBetaald.HasValue && (InschrijfGeld.Value-BedragBetaald.Value<=1000)));
            }
        }


        /// <summary>
        /// Het te betalen bedrag.
        /// </summary>
        public int? BedragTeBetalen {
            get {
                // Geef het verschil terug tussen te betalen bedrag en betaald bedrag, als beide zijn ingevuld.
                if (InschrijfGeld.HasValue && BedragBetaald.HasValue) {
                    return InschrijfGeld.Value-BedragBetaald.Value;
                }

                // Als geen betaald bedrag is ingevuld geef dan het inschrijf geld terug.
                if (InschrijfGeld.HasValue) {
                    return InschrijfGeld.Value;
                }

                // Geef anders onbekend bedrag op.
                return null;
            }
        }

        [Required(ErrorMessage = "Selecteer betaling")]
        public string BankCode { get; set; }

        protected List<SelectListItem> _bankList;

        public List<SelectListItem> BankList {
            get {
                if (_bankList==null) {
                    _bankList = SisowIdealHandler.GetIssuerList();
                }
                return _bankList;
            }
        }

        [Required(ErrorMessage = "Alleen iDeal betaling mogelijk")]
        public PaymentType PaymentType { get; set; }

        protected List<SelectListItem> _paymentTypeList;

        public List<SelectListItem> PaymentTypeList {
            get {
                if (_paymentTypeList==null) {
                    _paymentTypeList = new List<SelectListItem> {
                        new SelectListItem(),
                        new SelectListItem() { Selected = true, Text="iDeal", Value = "1" }
                    };
                }
                return _paymentTypeList;
            }
        }


        public string ReturnUrl {
            get {
                HttpRequest request = HttpContext.Current.Request;
                string result = Uri.EscapeDataString(request.Url.Scheme + System.Uri.SchemeDelimiter + request.Url.Host 
                        + (request.Url.IsDefaultPort ? "" : (":" + request.Url.Port)) + "/Inschrijvingen/Aangemeld");
                return result;
            }
        }


        public string SisowUrl {
            get {
                string result = null;
                if (!string.IsNullOrEmpty(ReturnUrl)) {
                    result = SisowIdealHandler.DetermineSisowGetUrl(InschrijfGeld.Value, 
                        ParticipationId.ToString(), string.Format("{0} H3RE", Voornaam), ReturnUrl, BankCode);
                }
                return result;
            }
        }
    }
}
