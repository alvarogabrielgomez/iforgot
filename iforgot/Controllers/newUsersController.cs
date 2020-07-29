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
    public class newUsersController : Controller
    {
        private _Database db = new _Database();

        // GET: newUsers
        public ActionResult Index()
        {
            return View(db.newUsers.ToList());
        }

        // GET: newUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            newUsers newUsers = db.newUsers.Find(id);
            if (newUsers == null)
            {
                return HttpNotFound();
            }
            return View(newUsers);
        }

        // GET: newUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: newUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "newUser_id,newUser_email,newUser_token,newUser_expires")] newUsers newUsers)
        {
            if (ModelState.IsValid)
            {
                db.newUsers.Add(newUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newUsers);
        }

        // GET: newUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            newUsers newUsers = db.newUsers.Find(id);
            if (newUsers == null)
            {
                return HttpNotFound();
            }
            return View(newUsers);
        }

        // POST: newUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "newUser_id,newUser_email,newUser_token,newUser_expires")] newUsers newUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newUsers);
        }

        // GET: newUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            newUsers newUsers = db.newUsers.Find(id);
            if (newUsers == null)
            {
                return HttpNotFound();
            }
            return View(newUsers);
        }

        // POST: newUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            newUsers newUsers = db.newUsers.Find(id);
            db.newUsers.Remove(newUsers);
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
