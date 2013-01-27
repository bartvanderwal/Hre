using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Net.Mail;
using System.IO;
using System.Web.Security;
using System.Threading;
using System.Globalization;
using HRE.Models.Newsletters;
using HRE.Models;
using HRE.Common;
using HRE.Business;
using HRE.Dal;

namespace HRE.Controllers {

    [Authorize(Roles="Admin")]
    public class NewsletterController : Controller {
        
        NewsletterRepository nr = new NewsletterRepository();


        public ActionResult Index(string id) {
            if (string.IsNullOrEmpty(id)) {
                return View(NewsletterRepository.GetNewsletterList());
            }
            return View(NewsletterRepository.GetNewsletterList(int.Parse(id)));
        }


        public ActionResult CreateNewsletter(string id) {
            if (string.IsNullOrEmpty(id)) {
                NewsletterViewModel nvm = new NewsletterViewModel();
                nvm.Created = DateTime.Now;
                nvm.Sent = null;
                nvm.Updated = null;
                nvm.SequenceNumber = 1;

                nvm.Items = new List<NewsletterItemViewModel>() {
                    new NewsletterItemViewModel {
                        Title = "Nieuwsbrief item"
                    },
                    new NewsletterItemViewModel {
                        Title = "Nieuwsbrief item"
                    }
                };

                return View(nvm);
            } else {
                int ID = Convert.ToInt32(id);
                return View(NewsletterRepository.GetByID(ID));
            }
        }


        public ActionResult SendNewsletter(int id) {
            NewsletterViewModel nvm = NewsletterRepository.GetByID(id);
            SendNewsletterViewModel snl = new SendNewsletterViewModel {
                NewsletterID = Convert.ToInt32(id),
                Subject = nvm.Title
            };
            return View(snl);
        }


        [HttpPost]
        public ActionResult SendNewsletter(SendNewsletterViewModel snlvm) {
            NewsletterViewModel nvm = NewsletterRepository.GetByID(snlvm.NewsletterID);
            string Newsletter = this.RenderNewsletterViewToString("NewsletterLocalizations/NewsletterLayout", nvm);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(HreSettings.ReplyToAddress, HreSettings.ReplyToAddress);
            message.To.Add(new MailAddress(snlvm.TestEmail));
            message.Subject = nvm.Title;
            message.IsBodyHtml = true;
            message.Body = Newsletter;
            EmailSender.SendEmail(message, EmailCategory.Newsletter, null);

            return View(snlvm);
        }


        public ActionResult Send(int id) {
            NewsletterViewModel nvm = NewsletterRepository.GetByID(id);
            if (nvm.Sent == null) {
                List<LogonUserDal> users = nr.DetermineAddressees(nvm.Culture);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(HreSettings.ReplyToAddress);
                mm.Subject = nvm.Title;
                mm.IsBodyHtml = true;
                string newsletter = this.RenderNewsletterViewToString("NewsletterLocalizations/NewsletterLayout", nvm);
                newsletter.Replace("%ID%", nvm.ID.ToString());
                mm.Body = newsletter;

                foreach (LogonUserDal user in users) {
                    mm.To.Clear();
                    mm.To.Add(new MailAddress(user.EmailAddress));
                    mm.Body = mm.Body.Replace("%UNSUB%", Util.RC2Encryption(user.EmailAddress, HreSettings.EmaCypher, HreSettings.HiddenCypher));
                    mm.Body = mm.Body.Replace("%ADDRESSEE%", user.PrimaryAddress.Firstname);
                    mm.Body = mm.Body.Replace("%SOSLINK%", Util.RC2Encryption(user.EmailAddress, HreSettings.SosCypher, HreSettings.HiddenCypher));
                    EmailSender.SendEmail(mm, EmailCategory.Newsletter, nvm.ID);
                    
                    // Reset body of email so %UNSUB% can be replaced next iteration.
                    mm.Body = newsletter;
                }
                nvm.Sent = DateTime.Now;
                NewsletterRepository.UpdateNewsletter(nvm);
                ViewBag.message = "E-mails met succes verstuurd!";                
            } else {
                ViewBag.message = "Deze nieuwsbrief is al eens verstuurd, neem contact op met de technische jongens.";  
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewsletter(NewsletterViewModel nvm) {
            nvm.Updated = DateTime.Now;

            if (ModelState.IsValid) {
                if (nvm.ID == 0) {
                    NewsletterRepository.AddNewsletter(nvm);
                } else {
                    NewsletterRepository.UpdateNewsletter(nvm);
                }
            }

            return View(nvm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PreviewNewsletter(NewsletterViewModel nvm) {
            ViewBag.IsEmail = false;
            return View("NewsletterLocalizations/NewsletterLayout", nvm);
        }

        [HttpPost]
        public ActionResult NewsletterAdresses(int id)
        {
            return PartialView("_NewsletterAdresses", nr.DetermineAddressees(id));
        }

        // [AllowAnonymous]
        public ActionResult Display(int id) {
            NewsletterViewModel nvm = NewsletterRepository.GetByID(id);
            if (nvm != null) {
                ViewBag.IsEmail = false;
                return View("NewsletterLocalizations/NewsletterLayout", nvm);
            } else {
                return RedirectToAction("Index", "Home");
            }
        }


        // [AllowAnonymous]
        public ActionResult Unsubscribe(string id) {
            string email = System.Web.HttpUtility.UrlDecode(id);
            try {
                email = Util.RC2Decryption(email, HreSettings.EmaCypher, HreSettings.HiddenCypher);
            } catch (Exception) {
                ViewBag.Message = "Ongeldig e-mail adres";
                return View();
            }

            LogonUserDal user = LogonUserDal.CreateOrRetrieveUser(email);

            if (user != null) {
                if (user.IsMailingListMember.HasValue && user.IsMailingListMember.Value) {
                    user.IsMailingListMember = false;
                    user.Save();
                    ViewBag.Message = email + ", U bent succesvol aangemeld voor de nieuwsbrief!";
                } else {
                    ViewBag.Message = email + "U bent succesvol afgemeld voor de nieuwsbrief!";
                }
            } else {
                ViewBag.Message = "Deze gebruiker is niet gevonden.";
            }

            return View();
        }
    }
}


public static partial class ControllerExtensions {
    public static string RenderNewsletterViewToString(this ControllerBase controller, string Path, object model, bool IsEmail = true) {
        if (string.IsNullOrEmpty(Path))
            Path = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        controller.ViewBag.IsEmail = IsEmail;

        using (StringWriter sw = new StringWriter()) {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, Path);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            // copy model state items to the html helper 
            foreach (var item in viewContext.Controller.ViewData.ModelState)
                if (!viewContext.ViewData.ModelState.Keys.Contains(item.Key)) {
                    viewContext.ViewData.ModelState.Add(item);
                }

            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
        }
    }
}