using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;

namespace HRE.Controllers {
    public class HtmlController : BaseController {

        public ActionResult Index() {
            string path = Url.Content("~/Content/html/index.html");
            return Redirect(path);
        }


        // Alias.
        public ActionResult Rest(string urlPart) {
            return new FilePathResult(urlPart, "text/html");
        }

        
    }
}
