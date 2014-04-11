using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;

namespace HRE.Controllers {
    public class HomeController : BaseController {

        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Home;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }
        
        public new List<string> SubMenuItems {
            get { 
                return new List<string> {
                    AppConstants.HomeWelkom,
                    AppConstants.HomeParcours,
                    AppConstants.HomeHistorie,
                    AppConstants.HomePartners,
                    AppConstants.HomeFaq,
                    AppConstants.HomeContact
                };
            }
        }


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

            // Initialise(AppConstants.HomeWelkom);
            // return View();
        }


        // Alias.
        public ActionResult Welkom() {
            Initialise(AppConstants.HomeWelkom);
            return RedirectToAction("Index");
        }

        
        public ActionResult Parcours() {
            Initialise(AppConstants.HomeParcours);
            return View();
        }

  
        public ActionResult Parcours2D() {
            Initialise(AppConstants.HomeParcours2D);
            return View();
        }


        public ActionResult Parcours3D() {
            Initialise(AppConstants.HomeParcours3D);
            return View();
        }


        public ActionResult Kaart() {
            Initialise(AppConstants.HomeParcours);
            return View();
        }

        public ActionResult Partners() {
            Initialise(AppConstants.HomePartners);
            return View();
        }

        // Pagina over 'Organisatie' valt onder zelfde submenu item als pagina 'Partners' die er naar linkt.
        public ActionResult Organisatie() {
            Initialise(AppConstants.HomePartners);
            return View();
        }


        public ActionResult Historie() {
            Initialise(AppConstants.HomeHistorie);
            return View();
        }

        public ActionResult Faq() {
            Initialise(AppConstants.HomeFaq);
            return View();
        }

        // public ActionResult Sponsors() {
        //    Initialise(AppConstants.HomeSponsors);
        //    return View();
        // }


        public ActionResult Contact() {
            Initialise(AppConstants.HomeContact);
            return View(new ContactViewModel());
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel contactVM) {
            Initialise(AppConstants.HomeContact);

            if (!ModelState.IsValid) {
                return View(contactVM);
            }

            var contact = new Contact {
                From = contactVM.Afzender,
                Subject = "#HRE Contact form: " + contactVM.Onderwerp,
                Message = "Dit heeft de gebruiker ingevuld:<br/>" + contactVM.Bericht
            };

            new Email().Send(contact);

            return RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm() {
            Initialise(AppConstants.HomeContact);
            return View();
        }
    }
}
