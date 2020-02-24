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
    public class DonneesCINsController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: DonneesCINs
        public ActionResult Index()
        {
            return View(db.DonneesCIN.ToList());
        }

        // GET: DonneesCINs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonneesCIN donneesCIN = db.DonneesCIN.Find(id);
            if (donneesCIN == null)
            {
                return HttpNotFound();
            }
            return View(donneesCIN);
        }

        // GET: DonneesCINs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonneesCINs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonneesCIN donneesCIN)
        {
            if (ModelState.IsValid)
            {
                db.DonneesCIN.Add(donneesCIN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donneesCIN);
        }

        // GET: DonneesCINs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonneesCIN donneesCIN = db.DonneesCIN.Find(id);
            if (donneesCIN == null)
            {
                return HttpNotFound();
            }
            return View(donneesCIN);
        }

        // POST: DonneesCINs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NOCIN,NOM,PERNOM,DTNAISS,DTVALID,ADRESS,ADRESS2,VILLE,PPERE,NMERE,Link")] DonneesCIN donneesCIN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donneesCIN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donneesCIN);
        }

        // GET: DonneesCINs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonneesCIN donneesCIN = db.DonneesCIN.Find(id);
            if (donneesCIN == null)
            {
                return HttpNotFound();
            }
            return View(donneesCIN);
        }

        // POST: DonneesCINs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonneesCIN donneesCIN = db.DonneesCIN.Find(id);
            db.DonneesCIN.Remove(donneesCIN);
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
