using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class ScrapeNtbIModel {

        public List<ScrapeNtbIEntryModel> Entries { get; set; }

        public int maxNumberOfScrapedItems { get; set; }
    }





}
