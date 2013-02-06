using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class ScrapeNtbIModel {

        public List<InschrijvingModel> Entries { get; set; }

        public int MaxNumberOfScrapedItems { get; set; }

        public bool OverrideLocallyUpdated { get; set; }
    }





}
