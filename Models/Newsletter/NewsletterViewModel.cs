using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HRE.Dal;
using HRE.Business;

namespace HRE.Models.Newsletters {
    public class NewsletterViewModel {
        public int ID { get; set; }
        
        
        [Required(ErrorMessage = "Het onderwerp van de nieuwsbrief is verplicht!")]
        [Display(Name = "Titel/Onderwerp")]
        public string Title { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum van aanmaken")]
        public DateTime DateCreated { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum laatst bijgewerkt")]
        public DateTime? DateUpdated { get; set; }
        

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        [Display(Name = "Datum van verzenden")]
        public DateTime? DateSent { get; set; }
        

        [Display(Name = "Land/Localisatie")]
        public int Culture { get; set; }
        

        [Display(Name = "Intro tekst")]
        public string IntroText { get; set; }
        

        [Display(Name = "Nieuwsbrief items")]
        public List<NewsletterItemViewModel> Items { get; set; } 


        [Display(Name = "Toevoegen persoonlijke inlog link?")]
        public bool IncludeLoginLink { get; set; }


        [Display(Name = "Opgegeven lidmaatschap?")]
        /// <summary>
        /// The subscription status.
        /// Default: only to members.
        /// </summary>
        public NewsletterSubscriptionStatus SubscriptionStatus { get; set; }


        [Display(Name = "Deelnemer in 2012? (=potential Early Bird)")]
        /// <summary>
        /// The subscription status.
        /// Default: only to members.
        /// </summary>
        public HREEventParticipantStatus Hre2012ParticipantStatus { get; set; }


        [Display(Name = "Als Early Bird ingeschreven?")]
        /// <summary>
        /// The subscription status.
        /// Default: to all, Early Birds and Non Early Birds.
        /// </summary>
        public EarlyBirdStatus EarlyBirdStatus { get; set; }


        [Display(Name = "Inschrijfgeld (al) betaald?")]
        /// <summary>
        /// Heeft de deelnemer (al) zijn/haar inschrijfgeld al (volledig) betaald?
        /// </summary>
        public EntryFeePaidStatus EntryFeePaidStatus { get; set; }


        public LogonUserDal CurrentLogonUser {get; set; }

    }
}