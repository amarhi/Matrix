using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Matrix.Models;

namespace Matrix.Controllers
{
    public class CSPsController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: CSPs
        public ActionResult Index()
        {
            return View(db.CSP.ToList());
        }

        // GET: CSPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP cSP = db.CSP.Find(id);
            if (cSP == null)
            {
                return HttpNotFound();
            }
            return View(cSP);
        }

        // GET: CSPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSPs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Libelle,Type")] CSP cSP)
        {
            if (ModelState.IsValid)
            {
                db.CSP.Add(cSP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSP);
        }

        // GET: CSPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP cSP = db.CSP.Find(id);
            if (cSP == null)
            {
                return HttpNotFound();
            }
            return View(cSP);
        }

        // POST: CSPs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Libelle,Type")] CSP cSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSP);
        }

        // GET: CSPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP cSP = db.CSP.Find(id);
            if (cSP == null)
            {
                return HttpNotFound();
            }
            return View(cSP);
        }

        // POST: CSPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSP cSP = db.CSP.Find(id);
            db.CSP.Remove(cSP);
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
