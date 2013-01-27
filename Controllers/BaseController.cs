using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;

namespace HRE.Controllers {
    /// <summary>
    /// Class with generic functionality for the HRE project.
    /// </summary>
    public class BaseController : Controller {

        public string ActiveMenuItem {
            get {
                return ViewBag.ActiveMenuItem;
            }
            set {
                ViewBag.ActiveMenuItem = value;
            }
        }


        public string ActiveSubMenuItem {
            get {
                return ViewBag.ActiveSubMenuItem;
            }
            set {
                ViewBag.ActiveSubMenuItem = value;
            }
        }

        public List<string> SubMenuItems {
            get {
                return ViewBag.SubMenuItems;
            }
            set {
                ViewBag.SubMenuItems = value;
            }
        }
    }
}
