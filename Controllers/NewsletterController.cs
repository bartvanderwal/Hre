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


    public class NewsletterController : Controller {
        
        NewsletterRepository nr = new NewsletterRepository();


        [Authorize(Roles="Admin")]
        public ActionResult Index(string id) {
            if (string.IsNullOrEmpty(id)) {
                return View(NewsletterRepository.GetNewsletterList());
            }
            return View(NewsletterRepository.GetNewsletterList(int.Parse(id)));
        }


        [Authorize(Roles="Admin")]
        public ActionResult CreateNewsletter(string id) {
            if (string.IsNullOrEmpty(id)) {
                NewsletterViewModel nvm = new NewsletterViewModel();
                nvm.DateCreated = DateTime.Now;
                nvm.DateSent = null;
                nvm.DateUpdated = null;

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


        [Authorize(Roles="Admin")]
        public ActionResult SendNewsletter(int newsletterId) {
            SendPersonalNewsletterViewModel snl = new SendPersonalNewsletterViewModel() {
                NewsletterId = newsletterId
            };
            return View(snl);
        }


        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult SendNewsletter(SendPersonalNewsletterViewModel spnvm) {
            // NewsletterViewModel nvm = NewsletterRepository.GetByID(snlvm.NewsletterId);
            spnvm.IsEmail=true;
            string Newsletter = this.RenderNewsletterViewToString("NewsletterTemplates/NewsletterTemplate", spnvm);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(HreSettings.ReplyToAddress, HreSettings.ReplyToAddress);
            message.To.Add(new MailAddress(LogonUserDal.GetByID(spnvm.UserId).EmailAddress));
            message.Subject = spnvm.Newsletter.Title;
            message.IsBodyHtml = true;
            message.Body = Newsletter;
            EmailSender.SendEmail(message, EmailCategory.Newsletter, spnvm.NewsletterId, spnvm.UserId);

            return View(spnvm);
        }


        [Authorize(Roles="Admin")]
        public ActionResult Sent(int Id) {
            SendPersonalNewsletterViewModel spnvm = new SendPersonalNewsletterViewModel();
            spnvm.NewsletterId = Id;
            spnvm.IsEmail = true;

            if (spnvm.Newsletter.DateSent == null) {
                List<LogonUserDal> users = LogonUserDal.GetNewsletterReceivers(spnvm.Newsletter.Audience);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(HreSettings.ReplyToAddress);
                mm.Subject = spnvm.Newsletter.Title;
                mm.IsBodyHtml = true;

                foreach (LogonUserDal user in users) {
                    spnvm.UserId = user.Id;
                    mm.Body = this.RenderNewsletterViewToString("NewsletterTemplates/NewsletterTemplate", spnvm);
                    mm.To.Clear();
                    mm.To.Add(new MailAddress(user.EmailAddress));
                    EmailSender.SendEmail(mm, EmailCategory.Newsletter, spnvm.Newsletter.ID, spnvm.UserId);                   
                }
                spnvm.Newsletter.DateSent = DateTime.Now;
                NewsletterRepository.UpdateNewsletter(spnvm.Newsletter);
                ViewBag.message = "E-mails met succes verstuurd!";                
            } else {
                ViewBag.message = "Deze nieuwsbrief is al eens verstuurd, neem contact op met de technische jongens.";  
            }

            return View();
        }


        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewsletter(NewsletterViewModel nvm) {
            nvm.DateUpdated = DateTime.Now;

            if (ModelState.IsValid) {
                if (nvm.ID == 0) {
                    NewsletterRepository.AddNewsletter(nvm);
                } else {
                    NewsletterRepository.UpdateNewsletter(nvm);
                }
            }

            return View(nvm);
        }

        
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PreviewNewsletter(NewsletterViewModel nvm) {
            ViewBag.IsEmail = false;
            return View("NewsletterTemplates/NewsletterTemplate", nvm);
        }


        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult NewsletterAdresses(int id) {
            return PartialView("_NewsletterAdresses", LogonUserDal.GetNewsletterReceivers(NewsletterRepository.GetByID(id).Audience));
        }


        // [AllowAnonymous]
        public ActionResult Display(int newsletterId) {
            NewsletterViewModel nvm = NewsletterRepository.GetByID(newsletterId);
            if (nvm != null) {
                SendPersonalNewsletterViewModel spnvm = new SendPersonalNewsletterViewModel();
                spnvm.NewsletterId = newsletterId;
                spnvm.UserId = LogonUserDal.GetByEmailAddress("bart@hetrondjeeilanden.nl").Id;
                return View("NewsletterTemplates/NewsletterTemplate", spnvm);
            } else {
                return RedirectToAction("Index", "Home");
            }
        }


        // [AllowAnonymous]
        public ActionResult Unsubscribe(string id) {
            string email = id;
            try {
                email = Common.Common.RC2Decryption(email, HreSettings.EmaCypher, HreSettings.HiddenCypher);
            } catch (Exception) {
                ViewBag.Message = "Ongeldig e-mail adres";
                return View();
            }

            LogonUserDal user = LogonUserDal.CreateOrRetrieveUser(email);

            if (user != null) {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(HreSettings.ReplyToAddress, HreSettings.ReplyToAddress);
                message.To.Add(new MailAddress("bartvanderwal@gmail.com"));
                message.To.Add(new MailAddress("pieter@hetrondjeeilanden.nl"));

                if (user.IsMailingListMember.HasValue && user.IsMailingListMember.Value) {
                    user.IsMailingListMember = false;
                    user.Save();

                    ViewBag.Message = email + ", U bent succesvol afgemeld voor de nieuwsbrief!";
                    message.Subject = "[HRE-Flessenpost] " + email + " heeft zich zojuist afgemeld voor de HRE nieuwsbrief";
                } else {
                    ViewBag.Message = email + ", U bent al afgemeld voor de nieuwsbrief!";
                    message.Subject = "[HRE-Flessenpost] " + email + " heeft zich zojuist afgemeld voor de HRE nieuwsbrief (maar had zich eigenlijk nooit aangemeld)";
                }
                EmailSender.SendEmail(message, EmailCategory.EmailUnsubscription, null);
            } else {
                ViewBag.Message = "Fout. Ongeldig unsubscribe link. Mail naar <a href=\"mailto:bart@hetrondjeeilanden.nl\">bart@hetrondjeeilanden.nl</a>";
            }

            return View();
        }
    }
}


public static partial class ControllerExtensions {
    public static string RenderNewsletterViewToString(this ControllerBase controller, string path, object model, bool IsEmail = true) {
        if (string.IsNullOrEmpty(path))
            path = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        controller.ViewBag.IsEmail = IsEmail;

        using (StringWriter sw = new StringWriter()) {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, path);
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