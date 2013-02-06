using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using HRE.Models;
using HRE.Business;
using HRE.Dal;
using HRE.Common;

namespace HRE.Controllers {
    public class AccountController : BaseController {

        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Account;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }
        
        public new List<string> SubMenuItems {
            get { 
                List<string> subMenuItems = new List<string> {
                    AppConstants.AccountWelkom,
                    AppConstants.AccountLogIn,
                    AppConstants.AccountRegistreer,
                };

                if (Membership.GetUser()!=null) {
                    subMenuItems.Add(AppConstants.AccountWijzigWW);
                }
                return subMenuItems;
            }
        }


        //
        // GET: /Account/LogOn
        //
        public ActionResult LogIn(string id) {
            string email = System.Web.HttpUtility.UrlDecode(id);
            if (!string.IsNullOrEmpty(id)) {
                try {
                    email = Util.RC2Decryption(email, HreSettings.EmaCypher, HreSettings.HiddenCypher);
                } catch (Exception) {
                    ViewBag.Message = "Ongeldige of verlopen inlog link";
                    return View();
                }
                LogonUserDal user = LogonUserDal.CreateOrRetrieveUser(email);

                if (user!=null) {
                    InschrijvingModel entryModel = new InschrijvingModel(user);
                    RedirectToAction("EarlyBird", "Meedoen", new { model = entryModel } );
                }
            }

            Initialise(AppConstants.AccountLogIn);
            return View();
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult LogIn(LogOnModel model, string returnUrl) {
            string login = string.Empty;
            Initialise(AppConstants.AccountLogIn);
            if (ModelState.IsValid) {
                if (!model.EmailAddress.Contains('@')) {
                    MembershipUserCollection members = Membership.FindUsersByEmail(model.EmailAddress + "@%");
                        if (members.Count==1) {
                        foreach(MembershipUser member in members) {
                            login = member.Email;
                            break;
                        }
                    } else {
                        ModelState.AddModelError("", "Geef het gehele e-mail adres op!");
                    }
                } else {
                    login = model.EmailAddress;
                }
                if (Membership.ValidateUser(login, model.Password)) {
                    FormsAuthentication.SetAuthCookie(login, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
                        return Redirect(returnUrl);
                    } else {
                        if (Roles.IsUserInRole(InschrijvingenRepository.ADMIN_ROLE_NAME)) {
                            return Redirect("/Admin/Dashboard.aspx");
                        } else {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                } else {
                    ModelState.AddModelError("", "De opgegeven gebruikersnaam en/of het paswoord is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogUit() {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Registreer() {
            Initialise(AppConstants.AccountRegistreer);
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Registreer(RegisterModel model) {
            if (ModelState.IsValid) {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.Email, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success) {
                    FormsAuthentication.SetAuthCookie(model.Email, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                } else {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult WijzigWW() {
            Initialise(AppConstants.AccountWijzigWW);
            return View();
        }


        public ActionResult Index() {
            Initialise(AppConstants.AccountWelkom);
            return RedirectToAction("Eilanders");
        }


        public ActionResult Welkom() {
            Initialise(AppConstants.Account);
            return RedirectToAction("Eilanders");
        }
        
        public ActionResult Eilanders() {
            Initialise(AppConstants.Account);
            return View();
        }

        
        // Alias.
        public ActionResult Overzicht() {
            Initialise(AppConstants.MeedoenOverzicht);
            return RedirectToAction("Index");
        }


        //
        // POST: /Account/ChangePassword.
        [Authorize]
        [HttpPost]
        public ActionResult WijzigWW(ChangePasswordModel model) {
            if (ModelState.IsValid) {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                } catch (Exception) {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded) {
                    return RedirectToAction("WijzigWWBevestiging");
                } else {
                    ModelState.AddModelError("", "Het huidige wachtwoord is incorrect of het nieuwe wachtwoord is niet geldig.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult WijzigWWBevestiging() {
            Initialise(AppConstants.AccountWijzigWW);
            return View();
        }


        public ActionResult Inschrijven(InschrijvingModel entryModel) {
            return View(entryModel);
        }


        public ActionResult EarlyBird(string id) {
            string email = id;
            try {
                email = Util.RC2Decryption(email, HreSettings.EmaCypher, HreSettings.HiddenCypher);
            } catch (Exception) {
                ViewBag.Message = "Ongeldige Early Bird™ link!<br/><br/> Was je deelnemer in 2012 en heb je problemen met inloggen vanuit de nieuwsbrief? Of heb je de nieuwsbrief helemaal niet ontvangen? Laat het ons weten: <a href=\"mailto:info@hetrondjeeilanden.nl\">info@hetrondjeeilanden.nl</a>.";
                return View();
            }

            EarlyBirdViewModel birdModel = new EarlyBirdViewModel();
            birdModel.User = LogonUserDal.CreateOrRetrieveUser(email);

            if (birdModel.User==null) {
                ViewBag.Message = "Ongeldige Early Bird™ link! <br/><br/> Was je deelnemer in 2012 en heb je problemen met inloggen vanuit de nieuwsbrief? Of heb je nieuwsbrief helemaal niet ontvangen? Laat het ons weten: <a href=\"mailto:info@hetrondjeeilanden.nl\">info@hetrondjeeilanden.nl</a>.";
                return View();
            } else {
                InschrijvingModel scrapeModel = new InschrijvingModel(birdModel.User);
                return View(scrapeModel);
            }
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    // return "User name already exists. Please enter a different user name.";
                    return "Gebruikersnaam bestaat al. Kies een andere gebruikersnaam.";
                case MembershipCreateStatus.DuplicateEmail:
                    // return "A user name for that e-mail address already exists. Please enter a different e-mail address.";
                    return "Er bestaat al een gebruikersnaam voor dit e-mail adres. Kies een ander e-mail adres.";

                case MembershipCreateStatus.InvalidPassword:
                    // return "The password provided is invalid. Please enter a valid password value.";
                    return "Het paswoord is ongeldig. Geef een geldig wachtwoord op.";

                case MembershipCreateStatus.InvalidEmail:
                    // return "The e-mail address provided is invalid. Please check the value and try again.";
                    return "Het e-mail adres is ongeldig. Controleer de waarde.";

                case MembershipCreateStatus.InvalidAnswer:
                    // return "The password retrieval answer provided is invalid. Please check the value and try again.";
                    return "De paswoord hint antwoord is ongeldig. Controleer de waarde.";

                case MembershipCreateStatus.InvalidQuestion:
                    // return "The password retrieval question provided is invalid. Please check the value and try again.";
                    return "De paswoord hint vraag is ongeldig. Controleer de waarde.";

                case MembershipCreateStatus.InvalidUserName:
                    // return "The user name provided is invalid. Please check the value and try again.";
                    return "De gebruikersnaam is ongeldig. Controleer de waarde.";

                case MembershipCreateStatus.ProviderError:
                    // return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    return "De authenticatie provided gaf een fout. Controleer je input. Neem contact met op via support@hetrondjeeilanden bij aanhoudende problemen.";

                case MembershipCreateStatus.UserRejected:
                    // return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    return "Het aanmaken van de gebruiker is geannuleerd. Controleer je input. Neem contact met ons op via support@hetrondjeeilanden.nl bij aanhoudende problemen.";

                default:
                    // return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    return "Er is een onbekende fout opgetreden. Controleer je input. Neem contact met ons op via support@hetrondjeeilanden.nl bij aanhoudende problemen.";
            }
        }
        #endregion
    }
}
