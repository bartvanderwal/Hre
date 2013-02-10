using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Business;
using HRE.Models;
using System.Web.Security;
using HRE.Dal;

namespace HRE.Controllers {
    public class ErrorController : BaseController {

        
        public ActionResult NietGevonden() {
            return View("_404");
        }


        public ActionResult GeenToegang() {
            return View("_401");
        }
        
        
        public ActionResult MeerRechtenVereist() {
            return View("NotAuthorized");
        }


        public ActionResult Oeps() {
            return View("DefaultError");
        }
    }
}
