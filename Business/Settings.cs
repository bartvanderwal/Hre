using System;
using System.Web.Configuration;


namespace HRE.Business {

    /// <summary>
    /// Contains settings.
    /// </summary>
    public static class Settings {
        public static bool IsDevelopmentEnvironment {
            get {
                return "True".Equals(WebConfigurationManager.AppSettings["IsDevelopmentEnvironment"], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static bool IsDebugModeAllowed {
            get {
                return "True".Equals(WebConfigurationManager.AppSettings["IsDebugModeAllowed"], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        
        public static string ContactToEmail {
            get {
                return WebConfigurationManager.AppSettings["ContactToEmail"];
            }
        }


        public static string EmaCypher {
            get {
                return WebConfigurationManager.AppSettings["EmaCypher"];
            }
        }


        public static string NewsletterTestMailAddresses {
            get {
                return WebConfigurationManager.AppSettings["NewsletterTestMailAddresses"];
            }
        }

        // Is this the development environment or not.
        public static bool IsDevelopment {
            get {
                return WebConfigurationManager.AppSettings["IsDevelopment"]=="True";
            }
        }
        
        // The path to the header image of the newsletter.
        public static string NewsletterHeaderImage {
            get {
                return WebConfigurationManager.AppSettings["NewsletterHeaderImage"];
            }
        }


        // Allow unencrypted link to unsubscribe/subscribe a user for the e-mail newsletter.
        public static bool AllowUnencryptedEmailForNewsletterSubscription {
            get {
                return WebConfigurationManager.AppSettings["AllowUnencryptedEmailForNewsletterSubscription"]=="True";
            }
        }

    }
}
