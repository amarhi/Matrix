using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Matrix.Models;
using System.IO;


namespace Matrix.Controllers
{
    public class DocumentsController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Emetteurs).Include(d => d.TypeDoc);
            return View(documents.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            ViewBag.CodeEmetteur = new SelectList(db.Emetteurs, "CodeEmetteur", "LibeEmetteur");
            ViewBag.Type = new SelectList(db.TypeDoc, "Code_Type", "Description");
            return View();
        }

        // POST: Documents/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Documents documents, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                documents.Filler = upload.FileName;
                db.Documents.Add(documents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeEmetteur = new SelectList(db.Emetteurs, "CodeEmetteur", "LibeEmetteur", documents.CodeEmetteur);
            ViewBag.Type = new SelectList(db.TypeDoc, "Code_Type", "Description", documents.Type);
            return View(documents);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodeEmetteur = new SelectList(db.Emetteurs, "CodeEmetteur", "LibeEmetteur", documents.CodeEmetteur);
            ViewBag.Type = new SelectList(db.TypeDoc, "Code_Type", "Description", documents.Type);
            return View(documents);
        }

        // POST: Documents/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Documents documents, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                documents.Filler = upload.FileName;
                db.Entry(documents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeEmetteur = new SelectList(db.Emetteurs, "CodeEmetteur", "LibeEmetteur", documents.CodeEmetteur);
            ViewBag.Type = new SelectList(db.TypeDoc, "Code_Type", "Description", documents.Type);
            return View(documents);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Documents documents = db.Documents.Find(id);
            db.Documents.Remove(documents);
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
