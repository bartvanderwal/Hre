using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HRE.Common;
using HRE.Dal;

namespace HRE.Models.Newsletters {
    public class SendPersonalNewsletterViewModel {
        
        [Required(ErrorMessage = "Vul een e-mail adres in.")]
        [Display(Name = "Test email adres:")]
        public string TestEmail { get; set; }
        
        public int NewsletterId { get; set; }
        
        public int LogonUserId { get; set; }


        public NewsletterViewModel Newsletter {
            get {
                return NewsletterRepository.GetByID(NewsletterId);
            }
        }

        public LogonUserDal LogonUser { 
            get {
                return LogonUserDal.GetByID(LogonUserId);
            }
        }


        public string PersonalLoginLink {
            get {
                return Util.RC2Encryption(LogonUserDal.GetByID(LogonUserId).EmailAddress, HreSettings.EmaCypher, HreSettings.HiddenCypher);
            }
        }

        public string UnsubscribeLink {
            get {
                return PersonalLoginLink;
            }
        }
        

        public string BaseDomain {
            get {
                return HreSettings.IsDevelopment ? "http://localhost:63647" : "http://www.hetrondjeeilanden.nl";
            }
        }

    }
}