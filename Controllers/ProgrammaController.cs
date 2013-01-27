using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Business;

namespace HRE.Controllers {
    public class ProgrammaController : BaseController {

        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Programma;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }

        public new List<string> SubMenuItems {
            get { return new List<string> {
                    AppConstants.ProgrammaSchema, 
                    AppConstants.ProgrammaTijdrit, 
                    AppConstants.ProgrammaAvondProgramma,
                    AppConstants.ProgrammaFinale,
                    AppConstants.ProgrammaKamperen                             
                };
            }
        }

        
        public ActionResult Index() {
            Initialise(AppConstants.ProgrammaSchema);
            return View();
        }

        // Alias.
        public ActionResult Schema() {
            Initialise(AppConstants.ProgrammaSchema);
            return RedirectToAction("Index");
        }


        // Alias voor SEO nav verplaatsen.
        public ActionResult Parcours() {
            Initialise(AppConstants.ProgrammaSchema);
            return RedirectToAction(AppConstants.HomeParcours, AppConstants.Home);
        }


        public ActionResult Tijdrace() {
            Initialise(AppConstants.ProgrammaTijdrit);
            return View();
        }


        public ActionResult Avond() {
            Initialise(AppConstants.ProgrammaAvondProgramma);
            return View();
        }

        public ActionResult Finale() {
            Initialise(AppConstants.ProgrammaFinale);
            return View();
        }

        public ActionResult Kamperen() {
            Initialise(AppConstants.ProgrammaKamperen);
            return View();
        }

    }
}
