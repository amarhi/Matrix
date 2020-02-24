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
    public class TypeDocsController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: TypeDocs
        public ActionResult Index()
        {
            return View(db.TypeDoc.ToList());
        }

        // GET: TypeDocs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeDoc typeDoc = db.TypeDoc.Find(id);
            if (typeDoc == null)
            {
                return HttpNotFound();
            }
            return View(typeDoc);
        }

        // GET: TypeDocs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeDocs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code_Type,Description")] TypeDoc typeDoc)
        {
            if (ModelState.IsValid)
            {
                db.TypeDoc.Add(typeDoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeDoc);
        }

        // GET: TypeDocs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeDoc typeDoc = db.TypeDoc.Find(id);
            if (typeDoc == null)
            {
                return HttpNotFound();
            }
            return View(typeDoc);
        }

        // POST: TypeDocs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code_Type,Description")] TypeDoc typeDoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeDoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeDoc);
        }

        // GET: TypeDocs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeDoc typeDoc = db.TypeDoc.Find(id);
            if (typeDoc == null)
            {
                return HttpNotFound();
            }
            return View(typeDoc);
        }

        // POST: TypeDocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TypeDoc typeDoc = db.TypeDoc.Find(id);
            db.TypeDoc.Remove(typeDoc);
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
