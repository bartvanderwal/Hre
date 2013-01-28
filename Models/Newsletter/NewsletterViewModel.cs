﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HRE.Dal;

namespace HRE.Models.Newsletters {
    public class NewsletterViewModel {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Het onderwerp van de nieuwsbrief is verplicht!")]
        [Display(Name = "Titel/Onderwerp")]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum van aanmaken")]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum laatst bijgewerkt")]
        public DateTime? Updated { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum van verzenden")]
        public DateTime? Sent { get; set; }
        
        [Display(Name = "Land/Localisatie")]
        public int Culture { get; set; }
        
        [Display(Name = "Volgnummer")]
        public int SequenceNumber { get; set; }
        
        [Display(Name = "Nieuwsbrief items")]
        public List<NewsletterItemViewModel> Items { get; set; } 

        [Display(Name = "Toevoegen persoonlijke inlog link?")]
        public bool IncludeLoginLink { get; set; }

        public bool IsEmail { get; set; }

        public LogonUserDal CurrentLogonUser {get; set; }

    }
}