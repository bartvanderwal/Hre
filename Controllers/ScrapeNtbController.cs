using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Security;
using HtmlAgilityPack;
using HRE.Data;
using HRE.Models;
using HRE.Business;
using System.Diagnostics;
using System.Threading;
using HRE.Common;

namespace HRE.Controllers {

    [Authorize(Roles="Admin")]
    public class ScrapeNtbController : BaseController {

        public ActionResult Index() {
            return RedirectToAction("Index", "Inschrijvingen");
        }
    }

}
