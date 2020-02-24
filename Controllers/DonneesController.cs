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
    public class DonneesController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: Donnees
        public ActionResult Index()
        {
            var donnees = db.Donnees.Include(d => d.CSP).Include(d => d.Documents);
            return View(donnees.ToList());
        }

        // GET: Donnees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donnees donnees = db.Donnees.Find(id);
            if (donnees == null)
            {
                return HttpNotFound();
            }
            return View(donnees);
        }

        // GET: Donnees/Create
        public ActionResult Create()
        {
            ViewBag.Code_CSP = new SelectList(db.CSP, "Code", "Libelle");
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle");
            return View();
        }

        // POST: Donnees/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code_CSP,CodeDoc,CodeData,LibData,TypeData,C7,C8")] Donnees donnees)
        {
            if (ModelState.IsValid)
            {
                db.Donnees.Add(donnees);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Code_CSP = new SelectList(db.CSP, "Code", "Libelle", donnees.Code_CSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", donnees.CodeDoc);
            return View(donnees);
        }

        // GET: Donnees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donnees donnees = db.Donnees.Find(id);
            if (donnees == null)
            {
                return HttpNotFound();
            }
            ViewBag.Code_CSP = new SelectList(db.CSP, "Code", "Libelle", donnees.Code_CSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", donnees.CodeDoc);
            return View(donnees);
        }

        // POST: Donnees/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code_CSP,CodeDoc,CodeData,LibData,TypeData,C7,C8")] Donnees donnees)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donnees).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Code_CSP = new SelectList(db.CSP, "Code", "Libelle", donnees.Code_CSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", donnees.CodeDoc);
            return View(donnees);
        }

        // GET: Donnees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donnees donnees = db.Donnees.Find(id);
            if (donnees == null)
            {
                return HttpNotFound();
            }
            return View(donnees);
        }

        // POST: Donnees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Donnees donnees = db.Donnees.Find(id);
            db.Donnees.Remove(donnees);
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
