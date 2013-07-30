using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace HRE.Common {

    public class HreSettings : SettingsBase {
        #region :: Constants

        public const string SEARCH = "Search";
        #endregion :: Constants

        /// <summary>
        /// True if the application is running on a (local) development environment.
        /// Default: False.
        /// NB: Deze kun je op development ook tijdelijk op False zetten om lokaal de staging omgeving te simuleren qua testen van staging urls.
        /// </summary>
        public static bool IsDevelopment {
            get {
                return ReadBoolSetting("IsDevelopment");
            }
        }


        /// <summary>
        /// True if the application is running on the production environment.
        /// Default: False.
        /// </summary>
        public static bool IsProduction {
            get {
                return ReadBoolSetting("IsProduction");
            }
        }


        /// <summary>
        /// Voor tonen van de Google 'Plus one' knop.
        /// Default: False.
        /// </summary>
        public static bool ShowGooglePlusOneButton {
            get {
                return ReadBoolSetting("ShowGooglePlusOneButton");
            }
        }

        /// <summary>
        /// Voor tonen van de 'Tweet this' knop.
        /// Default: False.
        /// </summary>
        public static bool ShowTweetThisButton {
            get {
                return ReadBoolSetting("ShowTweetThisButton");
            }
        }


        [ConfigurationProperty("EmaCypher", DefaultValue = "HreC1ph3r", IsRequired = false)]
        public static string EmaCypher {
            get {
                return ReadStringSetting("EmaCypher", "HreC1ph3r");
            }
        }


        /// <summary>
        /// The encryption key for the SOS code (Special Offer Subscription).
        /// </summary>
        [ConfigurationProperty("SosCypher", DefaultValue = "H2reEarlybird", IsRequired = false)]
        public static string SosCypher {
            get {
                return ReadStringSetting("EmaCypher", "H2reEarlybird");
            }
        }

        /// <summary>
        /// Minify the CSS and Javascript also on the development environment. 
        /// Please note: On Non development environments minification is always on. This setting cannot be used to override this (set compilation debug=true instead).
        /// Default: False.
        /// </summary>
        public static bool MinifyCssAndJavaScript {
            get {
                return ReadBoolSetting("MinifyCssAndJavaScript");
            }
        }

        /// <summary>
        /// Use a Content Develivery Network (CDN) or not. 
        /// For instance for javascript library for which a CDN Url is available, defined AND set (in BundleConfig.cs, like for instance jQuery) 
        /// the Content Delivery URL is used to speed up page load.
        /// Default: False.
        /// </summary>
        public static bool UseCdn {
            get {
                return ReadBoolSetting("UseCdn");
            }
        }


        /// <summary>
        /// The user name voor ntbinschrijvingen.nl. 
        /// Default value: "SWW".
        /// </summary>
        public static string NtbIUsername {
            get {
                return ReadStringSetting("NtbIUsername", "some_NtbIUsername");
            }
        }


        /// <summary>
        /// The password voor ntbinschrijvingen.nl. 
        /// Default value: "18jan2012".
        /// </summary>
        public static string NtbIPassword {
            get {
                return ReadStringSetting("NtbIPassword", "some_NtbIPassword");
            }
        }
        

        /// <summary>
        /// The reply to address for automatically generated e-mails (like the newsletter). 
        /// Default value: "info@hetrondjeeilanden.nl".
        /// </summary>
        public static string ReplyToAddress {
            get {
                return ReadStringSetting("ReplyToAddress", "info@hetrondjeeilanden.nl");
            }
        }


        /// <summary>
        /// The address to CC subscription mails to, as a backup :). 
        /// </summary>
        public static string ConfirmationsCCAddress {
            get {
                return ReadStringSetting("ConfirmationsCCAddress", "info@hetrondjeeilanden.nl");
            }
        }


        /// <summary>
        /// The *hidden* key for encryption, it doesnt really have to super secure because there are multiple salts
        /// Default: yordiisdaman!.
        /// </summary>
        public static string HiddenCypher {
            get {
                return ReadStringSetting("HiddenCypher", "yordiisdaman!");
            }
        }


        /// <summary>
        /// Should a readmore link be shown or not.
        /// Default: False.
        /// </summary>
        public static bool UseReadMoreLinks {
            get {
                return ReadBoolSetting("UseReadMoreLinks");
            }
        }


        /// <summary>
        /// The number of characters a text can be before it is cut off with a 'read more..' link.
        /// Default: 100.
        /// </summary>
        public static int CharacterLengthForReadMore {
            get {
                return ReadIntSetting("CharacterLengthForReadMore", 100);
            }
        }

        
        /// <summary>
        /// Het totaal aantal startplekken.
        /// Default: 500.
        /// </summary>
        public static int AantalStartPlekken {
            get {
                return ReadIntSetting("AantalStartPlekken", 500);
            }
        }


        /// <summary>
        /// Het aantal plekken op de rerervelijst.
        /// Default: 100.
        /// </summary>
        public static int AantalPlekkenReserveLijst {
            get {
                return ReadIntSetting("AantalPlekkenReserveLijst", 100);
            }
        }

        
        /// <summary>
        /// Het aantal Early Bird startplekken.
        /// Default: 200.
        /// </summary>
        public static int AantalEarlyBirdStartPlekken {
            get {
                return ReadIntSetting("AantalEarlyBirdStartPlekken", 200);
            }
        }


        /// <summary>
        /// De einddatum waarna er geen Early Bird korting meer wordt gegeven.
        /// </summary>
        public static DateTime EindDatumEarlyBirdKorting {
            get {
                return ReadDateTimeSetting("EindDatumEarlyBirdKorting", "15-03-2013");
            }
        }


        /// <summary>
        /// Openingsdatum algemene inschrijving.
        /// </summary>
        public static DateTime OpeningsdatumAlgemeneInschrijving {
            get {
                return ReadDateTimeSetting("OpeningsdatumAlgemeneInschrijving", "01-03-2013");
            }
        }


        /// <summary>
        /// Sluitingsdatum algemene inschrijving.
        /// </summary>
        public static DateTime SluitingssdatumAlgemeneInschrijving {
            get {
                return ReadDateTimeSetting("SluitingsdatumAlgemeneInschrijving", "31-07-2013");
            }
        }


        /// <summary>
        /// Het huidige - kale - deelnamebedrag (exclusief korting zoals early bird, en extra kosten zoals chip/licentie).
        /// Default: 2750.
        /// </summary>
        public static int HuidigeDeelnameBedrag {
            get {
                return ReadIntSetting("HuidigeDeelnameBedrag", 2750);
            }
        }


        /// <summary>
        /// Kosten van een daglicentie.
        /// Default: 230 (EUR 2,30).
        /// </summary>
        public static int KostenNtbDagLicentie {
            get {
                return ReadIntSetting("KostenNtbDagLicentie", 220);
            }
        }


        /// <summary>
        /// De kosten voor huur van een (gele) MyLaps chip.
        /// Default: 250.
        /// </summary>
        public static int KostenHuurMyLapsChipGeel {
            get {
                return ReadIntSetting("KostenHuurMyLapsChipGeel", 200);
            }
        }


        /// <summary>
        /// De korting voor Early Birds.
        /// Default: 750.
        /// </summary>
        public static int HoogteEarlyBirdKorting {
            get {
                return ReadIntSetting("HoogteEarlyBirdKorting", 750);
            }
        }


        /// <summary>
        /// Sisow password.
        /// </summary>
        public static string SisowPassword {
            get {
                return ReadStringSetting("SisowPassword");
            }
        }

        
        /// <summary>
        /// Sisow Merchant id.
        /// </summary>
        public static string SisowMerchantId {
            get {
                return ReadStringSetting("SisowMerchantId");
            }
        }

       
        /// <summary>
        /// Sisow Merchant key.
        /// </summary>
        public static string SisowMerchantKey {
            get {
                return ReadStringSetting("SisowMerchantKey");
            }
        }


        /// <summary>
        /// Sisow Merchant key.
        /// </summary>
        public static string MailAddressSecretary {
            get {
                return ReadStringSetting("MailAddressSecretary", "pieter@hetrondjeeilanden.nl");
            }
        }


        /// <summary>
        /// Specifies
        /// To be able to switch this of if SSL is no longer available, due to provider errors for example.
        /// </summary>
        public static bool IsSslAvailable {
            get {
                return ReadBoolSetting("IsSslAvailable", true);
            }
        }


        /// <summary>
        /// Het aantal personen per 'startschot' in de tijdrit serie. Default starten twee personen tegelijkertijd.
        /// </summary>
        public static int AantalPersonenPerStartschot {
            get {
                return ReadIntSetting("AantalPersonenPerStartschot", 2);
            }
        }


        /// <summary>
        /// Het aantal seconden twee twee startschots in de tijdrit serie. Default is dit 10 seconden.
        /// </summary>
        public static int AantalSecondenTussenStartschots {
            get {
                return ReadIntSetting("AantalSecondenTussenStartschots", 10);
            }
        }


        /// <summary>
        /// De datum en tijdstip van de 1e start van Het 2e Rondje Eilanden.
        /// </summary>
        public static DateTime DatumTijdstipH2RE {
            get {
                return ReadDateTimeSetting("DatumTijdstipH2RE", "3-8-2013 16:00:00");
            }
        }


    }
}