using System.Globalization;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using HRE.Dal;

namespace HRE.Models {

    public class NewsletterSubscribeViewModel : BaseRepository {

        public NewsletterSubscribeViewModel() { 
        }

        [Display]
        [Required]
        [StringLength(256)]
        public string UserEmailAddress { get; set; }

        public string UserMessage { get; set; }

        public string ToggleButtonText { 
            get {
                return Customer!=null && Customer.IsMailingListMember.HasValue && Customer.IsMailingListMember.Value ? 
                    "Afmelden" : "Aanmelden";
            }
        }

        public string ToggleButtonDisabled {
            get {
                return Customer==null ? "disabled=\"disabled\"" : string.Empty;
            }
        }


        private LogonUserDal _customer { get; set; }

        public LogonUserDal Customer { 
            get {
                // Lazyload.
                if (_customer == null && UserEmailAddress!=null) {
                    _customer = LogonUserDal.GetByEmailAddress(UserEmailAddress);
                }
                return _customer;
            }
        }
        

        /// <summary>
        /// Toggle the subscription (if off then turn on, if on then turn off).
        /// </summary>
        public void ToggleSubscription() {
            // If the user exists and his/her newsletter subscription is different from required then change it.
            if (Customer!=null) {
                Customer.IsMailingListMember = !Customer.IsMailingListMember;
                Customer.Save();
            }
        }


        /// <summary>
        /// Bepaal of een gebruiker is aangemeld voor de nieuwsbrief.
        /// Geeft true terug als de gebruiker zich heeft aangemeld voor de nieuwsbrief en false als deze zich heeft afgemeld.
        /// Geeft null terug als de gebruiker geen profiel heeft, de nieuwsbrief lidmaatschap niet expliciet is geset of de gebruiker niet gevonden is.
        /// </summary>
        private bool? GetSubscription(string userEmail) {
            if (Customer!=null) {
                return Customer.IsMailingListMember.HasValue ? Customer.IsMailingListMember.Value : false;
            }
            return null;
        }

    }
}