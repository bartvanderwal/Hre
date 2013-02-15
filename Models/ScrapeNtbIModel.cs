using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class ScrapeNtbIModel {

        public ScrapeNtbIModel() {
            // Default to HRE 2013!
            EventNumber = InschrijvingenRepository.H2RE_EVENTNR;
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


        /// <summary>
        /// The notes-to-all of all participants that filled it in.
        /// </summary>
        public string ParticipantRemarks {
            get {
                string result = "";
                foreach (var entry in Entries) {
                    if (!string.IsNullOrEmpty(entry.HebJeErZinIn)) {
                        result += entry.VolledigeNaam + ", <b>" + entry.Woonplaats + "</b>: '" + entry.HebJeErZinIn + "' - ";
                    }
                }

                if(string.IsNullOrEmpty(result)) {
                    return "";
                } else {
                    return "Hebben de H2RE Early Birds™ er al zin in? " + result;
                }
            }
        }
    }





}
