using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;
using HRE.Common;

namespace HRE.Controllers {
    /// <summary>
    /// Class with generic functionality for the HRE project.
    /// </summary>
    public class BaseController : Controller {


        public bool RequiresSsl {
            get {
                return HreSettings.IsSslAvailable && IsConfidentialPage;
            }
        }


        /// <summary>
        /// Return an http url from an https url.
        /// </summary>
        public ActionResult RedirectUrl() {
            string currentUrl = HttpContext.Request.Url.AbsoluteUri;
            
            // HTTPS coding ensure we send our vote over a secure connection.
            if (RequiresSsl && currentUrl.StartsWith("http:")) {
                return Redirect(UrlFromHttpToHttps(HttpContext.Request.Url.AbsoluteUri));
            }

            if (!RequiresSsl && currentUrl.StartsWith("https:")) {
                return Redirect(UrlFromHttpsToHttp(HttpContext.Request.Url.AbsoluteUri));
            }

            return Redirect(UrlFromHttpsToHttp(HttpContext.Request.Url.AbsoluteUri));
        }


        /// <summary>
        /// Return an http url from an https url.
        /// </summary>
        private static string UrlFromHttpToHttps(string url) {
            string newurl = url.Replace("http:", "https:");
            return newurl;
        }


        /// <summary>
        /// Return an https url from an http url.
        /// </summary>
        private static string UrlFromHttpsToHttp(string url) {
            string newurl = url.Replace("https:", "http:");
            return newurl;
        }

        public virtual bool IsConfidentialPage {
            get {
                return false;
            }
        }


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
