using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HRE.Common;
using HRE.Dal;

namespace HRE.Models.Newsletters {
    public class SendPersonalNewsletterViewModel {
        
        [Display(Name = "Test gebruiker")]
        public int? TestUserId { get; set; }
        
        [Display(Name = "Gebruiker")]
        public int? SingleUserId { get; set; }

        private int _userId { get; set; }

        /// <summary>
        /// The user is the User previously set, or if not the single user selected, or if not the test user selected, or 0 otherwise.
        /// </summary>
        public int UserId { 
            get {
                if (_userId==0) {
                    _userId = SingleUserId.HasValue ? SingleUserId.Value : (TestUserId.HasValue ? TestUserId.Value : 0);
                }
                return _userId;
            }
            set {
                _userId = value;
            }
        }

        public int NewsletterId { get; set; }


        private NewsletterViewModel _newsletter;


        public NewsletterViewModel Newsletter {
            get {
                if(_newsletter==null && NewsletterId!=0) {
                    _newsletter = NewsletterRepository.GetByID(NewsletterId);
                }
                return _newsletter;
            }
            set {
                _newsletter = value;
            }
        }

        public LogonUserDal User { 
            get {
                return LogonUserDal.GetByID(UserId);
            }
        }

        public bool IsEmail { get; set; }

        public string PersonalLoginLink {
            get {
                return Common.Common.RC2Encryption(LogonUserDal.GetByID(UserId).EmailAddress, HreSettings.EmaCypher, HreSettings.HiddenCypher);
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