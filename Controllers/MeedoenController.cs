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
    public class MeedoenController : BaseController {

        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Meedoen;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }
        
        public new List<string> SubMenuItems {
            get { 
                return new List<string> {
                    AppConstants.MeedoenOverzicht, 
                    AppConstants.MeedoenInschrijven, 
                    AppConstants.MeedoenVrijwilligers,
                    AppConstants.MeedoenDeelnemers,
                    AppConstants.MeedoenReglementen};
            }
        }

        
        public ActionResult Index() {
            Initialise(AppConstants.MeedoenOverzicht);
            return View();
        }


        // Alias.
        public ActionResult Overzicht() {
            Initialise(AppConstants.MeedoenOverzicht);
            return RedirectToAction("Index");
        }


        public ActionResult Inschrijven() {
            Initialise(AppConstants.MeedoenInschrijven);
            return View();
        }

        public ActionResult Deelnemers() {
            Initialise(AppConstants.MeedoenDeelnemers);
            return View();
        }

        public ActionResult Aanmeldingen() {
            Initialise(AppConstants.MeedoenDeelnemers);
            return View();
        }

        public ActionResult Vrijwilligers() {
            Initialise(AppConstants.MeedoenVrijwilligers);
            return View(new ContactViewModel());
        }


        [HttpPost]
        public ActionResult Vrijwilligers(ContactViewModel contactVM) {
            Initialise(AppConstants.HomeContact);

            if (!ModelState.IsValid) {
                return View(contactVM);
            }

            var contact = new Contact {
                From = contactVM.Afzender,
                Subject = "#HRE Vrijwilligers form: " + contactVM.Onderwerp,
                Message = "Dit heeft de gebruiker ingevuld:<br/>" + contactVM.Bericht
            };

            new Email().Send(contact);

            return RedirectToAction("VrijwilligersConfirm");
        }


        public ActionResult VrijwilligersConfirm() {
            Initialise(AppConstants.MeedoenVrijwilligers);
            return View();
        }


        public ActionResult Reglementen() {
            Initialise(AppConstants.MeedoenReglementen);
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult HouMeOpDeHoogte(MeedoenModel model) {
            // Controleer of het e-mail adres geldig is.
            if (string.IsNullOrEmpty(model.Email) || !model.Email.IsValidEmail()) {
                ModelState.AddModelError("Email", "Vul een geldig e-mail adres in!");
                return View("Index");
            }

            // Controleer of er al een gebruiker met dit e-mail adres bestaat.
            var user = LogonUserDal.GetByEmailAddress(model.Email);
            if (user!=null) {
                // Zo ja, controleer of deze gebruiker de nieuwsbrief al ontvangt en geef dan een foutmelding.
                if (user.IsMailingListMember.HasValue && user.IsMailingListMember.Value) {
                    ModelState.AddModelError("email", "Dit e-mail adres ontvangt de e-mail nieuwsbrief al!");
                } else {
                    // Zo nee, maak de bestaande gebruiker dan lid van de e-mail nieuwsbrief.
                    user.SubscribeNewsletter();
                }
            // Als er nog geen gebruiker bestaat, maak dan een nieuwe - niet actieve - gebruiker aan en maak deze lid van de e-mail nieuwsbrief.
            } else {
                LogonUserDal.AddNotActiveUserWithNewsletterSubscription(model.Email);
            }

            // Toon beginscherm.
            return View("Index");
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EarlyBird(ScrapeNtbIEntryModel model) {
            return View(model);
        }

    }
}
