using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Matrix.Models;
using System.Web.Script.Serialization;
using kit = TallComponents.PDF;
using rast = TallComponents.PDF.Rasterizer;
using System.Web.Routing;


namespace Matrix.Controllers
{
    
    public class CSP_DOCController : Controller
    {
        
        private MatrixEntities db = new MatrixEntities();

        // GET: CSP_DOC
        public ActionResult Index()
        {
            var cSP_DOC = db.CSP_DOC.Include(c => c.CSP).Include(c => c.Documents);
            return View(cSP_DOC.ToList());
        }

        // GET: CSP_DOC/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP_DOC cSP_DOC = db.CSP_DOC.Find(id);
            if (cSP_DOC == null)
            {
                return HttpNotFound();
            }
            return View(cSP_DOC);
        }

        // GET: CSP_DOC/Create
        public ActionResult Create()
        {
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle");
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle");
            return View();
        }

        // POST: CSP_DOC/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( CSP_DOC cSP_DOC)
        {
            if (ModelState.IsValid)
            {
                db.CSP_DOC.Add(cSP_DOC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", cSP_DOC.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", cSP_DOC.CodeDoc);
            return View(cSP_DOC);
        }

        // GET: CSP_DOC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP_DOC cSP_DOC = db.CSP_DOC.Find(id);
            if (cSP_DOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", cSP_DOC.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", cSP_DOC.CodeDoc);
            return View(cSP_DOC);
        }

        // POST: CSP_DOC/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CSP_DOC cSP_DOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSP_DOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", cSP_DOC.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", cSP_DOC.CodeDoc);
            return View(cSP_DOC);
        }

        // GET: CSP_DOC/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSP_DOC cSP_DOC = db.CSP_DOC.Find(id);
            if (cSP_DOC == null)
            {
                return HttpNotFound();
            }
            return View(cSP_DOC);
        }

        // POST: CSP_DOC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CSP_DOC cSP_DOC = db.CSP_DOC.Find(id);
            db.CSP_DOC.Remove(cSP_DOC);
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
