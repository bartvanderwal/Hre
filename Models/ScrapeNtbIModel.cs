using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class ScrapeNtbIModel {

        public ScrapeNtbIModel() {
            if(IsAdmin) {
                EventNumber = InschrijvingenRepository.HRE_EVENTNR;
            } else {
                EventNumber = InschrijvingenRepository.H2RE_EVENTNR;
            }
        }

        public List<InschrijvingModel> Entries { get; set; }

        public int MaxNumberOfScrapedItems { get; set; }

        public bool OverrideLocallyUpdated { get; set; }

        public string EventNumber { get; set; }

        public int UserIdToDelete { get; set; }

        public string Message { get; set; }

        public bool IsAdmin { 
            get {
                return Roles.IsUserInRole("Admin");
            }
        }
    }





}
