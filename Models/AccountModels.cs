using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class ChangePasswordModel {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Huidige wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet tenminste {2} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig nieuw wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord is niet hetzelfde als de bevestiging.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail adres")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Blijf ingelogd")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel {
        
        [Display(Name = "Gebruikersnaam")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get { 
            return Email;
            }
        } 

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail adres (=login naam)")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet tenminste {2} karakters lang zijn.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en de bevestiging ervan komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
