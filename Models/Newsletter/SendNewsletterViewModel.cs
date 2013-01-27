using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HRE.Models.Newsletters {
    public class SendNewsletterViewModel {
        
        [Required(ErrorMessage = "Vul een e-mail adres in.")]
        [Display(Name = "Test email adres:")]
        public string TestEmail { get; set; }
        
        public int NewsletterID { get; set; }

        [Display(Name = "Onderwerp:")]
        public string Subject { get; set; }
    }
}