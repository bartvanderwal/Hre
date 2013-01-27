using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRE.Controllers {
    public class OudFaqController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "";

            return View();
        }
    }
}
