using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;

namespace HRE.Controllers {
    public class StaticHtmlController : BaseController {

        public ActionResult Index(string first) {
            string path;
            if (first==null || !first.EndsWith("index.html")) {
                if (string.IsNullOrEmpty(first)) {
                    path = Url.Content("~/index.html");
                } else {
                    first += "/index.html";
                    path = Url.Content("~/" + first);
                }
                return Redirect(path);
            }
            path = Url.Content("~/" + first);
            return Redirect(first);

            // return View();
        }


    }
}
