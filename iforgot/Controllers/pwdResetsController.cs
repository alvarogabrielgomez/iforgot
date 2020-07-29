using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iforgot.Models;

namespace iforgot.Controllers
{
    public class pwdResetsController : Controller
    {
        private _Database db = new _Database();

        // GET: pwdResets
        public ActionResult Index()
        {
            return View(db.pwdReset.ToList());
        }

        // GET: pwdResets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pwdReset pwdReset = db.pwdReset.Find(id);
            if (pwdReset == null)
            {
                return HttpNotFound();
            }
            return View(pwdReset);
        }

        // GET: pwdResets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pwdResets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pwdReset_id,pwdReset_email,pwdReset_selector,pwdReset_token,pwdReset_expires")] pwdReset pwdReset)
        {
            if (ModelState.IsValid)
            {
                db.pwdReset.Add(pwdReset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pwdReset);
        }

        // GET: pwdResets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pwdReset pwdReset = db.pwdReset.Find(id);
            if (pwdReset == null)
            {
                return HttpNotFound();
            }
            return View(pwdReset);
        }

        // POST: pwdResets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pwdReset_id,pwdReset_email,pwdReset_selector,pwdReset_token,pwdReset_expires")] pwdReset pwdReset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pwdReset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pwdReset);
        }

        // GET: pwdResets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pwdReset pwdReset = db.pwdReset.Find(id);
            if (pwdReset == null)
            {
                return HttpNotFound();
            }
            return View(pwdReset);
        }

        // POST: pwdResets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pwdReset pwdReset = db.pwdReset.Find(id);
            db.pwdReset.Remove(pwdReset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
