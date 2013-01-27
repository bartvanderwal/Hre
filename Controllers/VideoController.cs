using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Business;

namespace HRE.Controllers {
    public class VideoController : BaseController {

        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Video;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }
        
        public new List<string> SubMenuItems {
            get { 
                return new List<string> {
                    AppConstants.VideoTweets, 
                    AppConstants.VideoYouTube,
                    AppConstants.VideoUitslagen
                };
            }
        }

        public ActionResult Index() {
            Initialise(AppConstants.VideoYouTube);
            return View();
        }


        // Alias.
        public ActionResult Tweets() {
            return View();
        }


        public ActionResult YouTube() {
            Initialise(AppConstants.VideoYouTube);
            return View();
        }


        // public ActionResult Media() {
        //    Initialise(AppConstants.NieuwsMedia);
        //    return View();
        //}

        public ActionResult Uitslagen() {
           Initialise(AppConstants.VideoUitslagen);
           return View();
        }

    }
}
