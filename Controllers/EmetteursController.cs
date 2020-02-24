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
    public class EmetteursController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: Emetteurs
        public ActionResult Index()
        {
            return View(db.Emetteurs.ToList());
        }

        // GET: Emetteurs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emetteurs emetteurs = db.Emetteurs.Find(id);
            if (emetteurs == null)
            {
                return HttpNotFound();
            }
            return View(emetteurs);
        }

        // GET: Emetteurs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emetteurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodeEmetteur,LibeEmetteur,TypeEmetteur")] Emetteurs emetteurs)
        {
            if (ModelState.IsValid)
            {
                db.Emetteurs.Add(emetteurs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emetteurs);
        }

        // GET: Emetteurs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emetteurs emetteurs = db.Emetteurs.Find(id);
            if (emetteurs == null)
            {
                return HttpNotFound();
            }
            return View(emetteurs);
        }

        // POST: Emetteurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodeEmetteur,LibeEmetteur,TypeEmetteur")] Emetteurs emetteurs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emetteurs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emetteurs);
        }

        // GET: Emetteurs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emetteurs emetteurs = db.Emetteurs.Find(id);
            if (emetteurs == null)
            {
                return HttpNotFound();
            }
            return View(emetteurs);
        }

        // POST: Emetteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Emetteurs emetteurs = db.Emetteurs.Find(id);
            db.Emetteurs.Remove(emetteurs);
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
