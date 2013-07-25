using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Data;
using HRE.Models;

namespace HRE.Controllers {
    public class emailauditsController : Controller {
		private readonly IemailauditRepository emailauditRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public emailauditsController() : this(new emailauditRepository()) {
        }

        public emailauditsController(IemailauditRepository emailauditRepository) {
			this.emailauditRepository = emailauditRepository;
        }

        //
        // GET: /emailaudits/

        public ViewResult Index() {
            return View(emailauditRepository.All);
        }

        //
        // GET: /emailaudits/Details/5

        public ViewResult Details(int id) {
            return View(emailauditRepository.Find(id));
        }

        //
        // GET: /emailaudits/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /emailaudits/Create

        [HttpPost]
        public ActionResult Create(emailaudit emailaudit)
        {
            if (ModelState.IsValid) {
                emailauditRepository.InsertOrUpdate(emailaudit);
                emailauditRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /emailaudits/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(emailauditRepository.Find(id));
        }

        //
        // POST: /emailaudits/Edit/5

        [HttpPost]
        public ActionResult Edit(emailaudit emailaudit)
        {
            if (ModelState.IsValid) {
                emailauditRepository.InsertOrUpdate(emailaudit);
                emailauditRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /emailaudits/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(emailauditRepository.Find(id));
        }

        //
        // POST: /emailaudits/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            emailauditRepository.Delete(id);
            emailauditRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                emailauditRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

