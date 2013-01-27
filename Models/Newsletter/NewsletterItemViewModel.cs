using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HRE.Models.Newsletters {
    public class NewsletterItemViewModel {

        public int ID { get; set; }

        public NewsletterViewModel NVM { get; set; }

        public int SequenceNumber { get; set; }

        [Display(Name="Titel")]
        public string Title { get; set; }

        [Display(Name = "Sub Titel")]
        public string SubTitle { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Afbeelding")]
        public string ImagePath { get; set; }

        [Display(Name = "Icon")]
        public string IconImagePath { get; set; }
    }
}