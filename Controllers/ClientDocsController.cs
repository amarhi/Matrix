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
    public class ClientDocsController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: ClientDocs
        public ActionResult Index()
        {
            var clientDoc = db.ClientDoc.Include(c => c.CSP).Include(c => c.Documents).Include(c => c.DonneesCIN);
            return View(clientDoc.ToList());
        }

        // GET: ClientDocs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDoc clientDoc = db.ClientDoc.Find(id);
            if (clientDoc == null)
            {
                return HttpNotFound();
            }
            return View(clientDoc);
        }

        // GET: ClientDocs/Create
        public ActionResult Create()
        {
            ViewBag.NbDoc = db.CSP_DOC.Count();
            ViewBag.docs = new SelectList(db.CSP_DOC, "Id", "CodeDoc");
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle");
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle");
            ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN");
            
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            HttpPostedFileBase upload = Request.Files[0];
            Guid guid = Guid.NewGuid();
            kit.Document pdf = new kit.Document(upload.InputStream);
            upload.SaveAs(Server.MapPath(string.Format("~/Upload/{0}.pdf", guid)));

            return PartialView("Index", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf });
        }

        // POST: ClientDocs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientDoc clientDoc)
        {
            string csp = clientDoc.CodeCSP;
            TempData["Idc"] = clientDoc.IdClient;
            TempData["Cs"] = clientDoc.CodeCSP;
            
            if (ModelState.IsValid)
            {
            /*     HttpPostedFileBase file = upload as HttpPostedFileBase;
                 Guid guid = Guid.NewGuid();
                 kit.Document pdf = new kit.Document(file.InputStream);
                //kit.Document doc = pdf;
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                clientDoc.UrlDoc = upload.FileName;
                db.ClientDoc.Add(clientDoc);
                db.SaveChanges(); */

             //   file.SaveAs(Server.MapPath(string.Format("~/Upload/{0}.pdf", guid)));


                ViewBag.NbDoc = db.CSP_DOC.Count();
                ViewBag.docs = new SelectList(db.CSP_DOC, "Id", "CodeDoc");
                ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", clientDoc.CodeCSP);
                ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", clientDoc.CodeDoc);
                ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN", clientDoc.IdDoc);
                
                TempData["Url"] = clientDoc.UrlDoc;
                TempData["Idc"] = clientDoc.IdClient;
                TempData["Cs"] = clientDoc.CodeCSP;

                //     return RedirectToAction("Details", new RouteValueDictionary(
                //         new { controller = "Depouillement", action = "Details", csp = csp, pdf = pdf, guid = guid  }));

                //   return PartialView("~/Views/Depouillement/_Details.cshtml", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf });
                return View();
            }   
            
            return View(clientDoc);
        }

        
        // GET: ClientDocs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDoc clientDoc = db.ClientDoc.Find(id);
            if (clientDoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.NbDoc = db.CSP_DOC.Count();
            ViewBag.docs = new SelectList(db.CSP_DOC, "Id", "CodeCSP");

            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", clientDoc.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", clientDoc.CodeDoc);
            ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN", clientDoc.IdDoc);
            return View(clientDoc);
        }

        // POST: ClientDocs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientDoc clientDoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientDoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", clientDoc.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", clientDoc.CodeDoc);
            ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN", clientDoc.IdDoc);
            return View(clientDoc);
        }

        // GET: ClientDocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDoc clientDoc = db.ClientDoc.Find(id);
            if (clientDoc == null)
            {
                return HttpNotFound();
            }
            return View(clientDoc);
        }

        // POST: ClientDocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientDoc clientDoc = db.ClientDoc.Find(id);
            db.ClientDoc.Remove(clientDoc);
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

