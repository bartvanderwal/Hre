using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRE.Models.Newsletters {
    public class NewsletterItemViewModel {

        public int ID { get; set; }

        public NewsletterViewModel NVM { get; set; }

        public int SequenceNumber { get; set; }

        [Display(Name="Titel")]
        public string Title { get; set; }

        [Display(Name = "Sub titel")]
        public string SubTitle { get; set; }

        [Display(Name = "Text")]
        [AllowHtml]
        public string Text { get; set; }

        [Display(Name = "Afbeelding")]
        public string ImagePath { get; set; }

        [Display(Name = "Icon")]
        public string IconImagePath { get; set; }

        [Display(Name = "Header HTML kleur")]
        public string HeadingHtmlColour { get; set; }
        }
}