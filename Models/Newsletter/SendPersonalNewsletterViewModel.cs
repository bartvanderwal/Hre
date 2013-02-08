﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HRE.Common;
using HRE.Dal;

namespace HRE.Models.Newsletters {
    public class SendPersonalNewsletterViewModel {
        
        [Required(ErrorMessage = "Kies een test geadresseerde.")]
        [Display(Name = "Test email adres:")]
        public int UserId { get; set; }
        
        public int NewsletterId { get; set; }


        public NewsletterViewModel Newsletter {
            get {
                return NewsletterRepository.GetByID(NewsletterId);
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