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
        public static bool ShowGooglePlusOneButtons {
            get {
                return ReadBoolSetting("ShowGooglePlusOneButtons");
            }
        }

        /// <summary>
        /// Voor tonen van de 'Tweet this' knop.
        /// Default: False.
        /// </summary>
        public static bool ShowTweetThisButtons {
            get {
                return ReadBoolSetting("ShowTweetThisButtons");
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
        /// The number of characters a vote (localization remark) can be before it is cut off with a 'read more..' link.
        /// Default: 100.
        /// </summary>
        public static int CharacterLengthForReadMore {
            get {
                return ReadIntSetting("CharacterLengthForReadMore", 100);
            }
        }


    }
}