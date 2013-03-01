using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using HRE.Business;
using HRE.Dal;

namespace HRE.Models {

    public class InschrijvingenModel {

        public InschrijvingenModel() {
            // Default to HRE 2013!
            EventNumber = InschrijvingenRepository.H2RE_EVENTNR;
        }

        private List<InschrijvingModel> _inschrijvingen { get; set; }


        public List<InschrijvingModel> Inschrijvingen { 
            get {
                if (_inschrijvingen==null) {
                    return InschrijvingenRepository.GetEntries(EventNumber, IsAdmin);
                }
                return _inschrijvingen;
            }
        }
       
        public int MaxNumberOfScrapedItems { get; set; }

        public bool OverrideLocallyUpdated { get; set; }

        public string EventNumber { get; set; }

        private SportsEventDal _event { get; set; }

        public SportsEventDal Event { 
            get {
                if (_event==null && !string.IsNullOrEmpty(EventNumber)) {
                    _event = SportsEventDal.GetByExternalId(EventNumber);
                }
                return _event;
            }
            set {
                _event = value;
            }
        }

        public int UserIdToDelete { get; set; }

        public string Message { get; set; }

        public bool IsAdmin { 
            get {
                return Roles.IsUserInRole("Admin");
            }
        }


        private string _particpationRemarks { get; set; }

        /// <summary>
        /// The notes-to-all of all participants that filled it in.
        /// </summary>
        public string ParticipantRemarks {
            get {
                if (_particpationRemarks == null) {
                
                    List<InschrijvingModel> inschrijvingenInRandomVolgorde = Common.Common.RandomizeList<InschrijvingModel>(Inschrijvingen);
                    foreach (var inschrijving in inschrijvingenInRandomVolgorde) {
                        if (!string.IsNullOrEmpty(inschrijving.HebJeErZinIn)) {
                            string entryText = inschrijving.HebJeErZinIn.Trim(); 
                            _particpationRemarks += string.Format(
                                "<span class=\"marquee-athlete-remark\">“ {0} ”</span> <span class=\"marquee-athlete\"> - {1}, {2} &nbsp&nbsp&nbsp&nbsp</span>", 
                                entryText, inschrijving.Voornaam, inschrijving.Woonplaats.ToLower().UppercaseFirst());
                        }
                    }
                }
                return _particpationRemarks;
            }
        }
    }
}
