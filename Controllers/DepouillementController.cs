using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Matrix.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using kit = TallComponents.PDF;
using rast = TallComponents.PDF.Rasterizer;
using System.Web.Routing;
using TallComponents.PDF;

namespace Matrix.Controllers
{
   
    public class DepouillementController : Controller
    {
        private MatrixEntities db = new MatrixEntities();

        // GET: Depouillement   
        [HttpPost]
        public ActionResult Index(string csp, Document pdf, Guid guid)
        {
     //       HttpPostedFileBase file = pdf as HttpPostedFileBase;
     //       Guid guid = gui;
     //       kit.Document pdf = new kit.Document(file.InputStream);
     //       file.SaveAs(Server.MapPath(string.Format("~/Upload/{0}.pdf", guid)));
            ViewBag.Idc = TempData["Idc"];
            ViewBag.Cs = TempData["Cs"];
            ViewBag.Url = TempData["Url"];
            var cSP_DOC = db.CSP_DOC.Where(c => c.CodeCSP == csp & c.CodeDoc != "Glob").ToList();
            ViewBag.data = cSP_DOC;

            //  HttpPostedFileBase file = Request.Files[0];
            // HttpPostedFileBase file = "~/Uploads" + ViewBag.Url[0];
            
          // Guid guid = Guid.NewGuid();

           //  kit.Document pdf = doc;
            //file.SaveAs(Server.MapPath(string.Format(ViewBag.Url, guid)));

           // return View(cSP_DOC.ToList());
            return View("Panel", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf });
        }

        
        
        public ActionResult Details(string csp)
        {
            ViewBag.Idc = TempData["Idc"];
            ViewBag.Cs = TempData["Cs"];
            ViewBag.Url = TempData["Url"];
            var cSP_DOC = db.CSP_DOC.Where(c => c.CodeCSP == csp & c.CodeDoc != "Glob").ToList();
            ViewBag.data = cSP_DOC;

                return PartialView("_Details", cSP_DOC.ToList());
        }



            public ActionResult Depouill(int? id, string cs) 
        {
            var cSP_DOC = db.CSP_DOC.Where(c => c.CodeCSP == cs).ToList(); 
            return View(cSP_DOC.ToList());
          //   Document = Document
          //      return Document et DocumentGuid
        }
        

        // GET: Depouillement/Create
        public ActionResult Create()
        {
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle");
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle");
            return View();
        }

        // POST: Depouillement/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CSP_DOC cSP_DOC)
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

        // GET: Depouillement/Edit/5
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

        // POST: Depouillement/Edit/5
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

        // GET: Depouillement/Delete/5
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

        // POST: Depouillement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CSP_DOC cSP_DOC = db.CSP_DOC.Find(id);
            db.CSP_DOC.Remove(cSP_DOC);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Panel()
        {
            return PartialView("_Panel", new PanelModel());
        }

        [HttpPost]
        public ActionResult Upload()
        {
            HttpPostedFileBase upload = Request.Files[0];
            Guid guid = Guid.NewGuid();
            kit.Document pdf = new kit.Document(upload.InputStream);
            upload.SaveAs(Server.MapPath(string.Format("~/Uploads/{0}", guid)));

            return PartialView("~/Views/Depouillement/_Panel.cshtml", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf });
        //    return PartialView("Index",  DocumentGuid = guid.ToString(), Document = pdf );
        }

        public class PanelPage
        {
            public string Guid { get; set; }
            public int Index { get; set; }
        }

        [HttpPost]
        public ActionResult Download(PanelPage[] pages)
        {
            string json = new JavaScriptSerializer().Serialize(pages);
            string id = Guid.NewGuid().ToString();
            string path = Server.MapPath(string.Format("~/Download/{0}.json", id));
            System.IO.File.WriteAllText(path, json);
            return Content(id);
        }

        public ActionResult Download(string id)
        {
            string path = Server.MapPath(string.Format("~/Download/{0}.json", id));
            string json = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            PanelPage[] pages = new JavaScriptSerializer().Deserialize<PanelPage[]>(json);

            Dictionary<string, FileStream> streams = new Dictionary<string, FileStream>();
            Dictionary<string, kit.Document> documents = new Dictionary<string, kit.Document>();

            kit.Document newPdf = new kit.Document();

            foreach (PanelPage page in pages)
            {
                FileStream file;
                kit.Document pdf;

                if (!streams.ContainsKey(page.Guid))
                {
                    file = new FileStream(Server.MapPath(string.Format("~/Upload/{0}.pdf", page.Guid)), FileMode.Open, FileAccess.Read);
                    streams[page.Guid] = file;

                    pdf = new kit.Document(file);
                    documents[page.Guid] = pdf;
                }
                else
                {
                    pdf = documents[page.Guid];
                }

                newPdf.Pages.Add(pdf.Pages[page.Index].Clone());
            }

            using (MemoryStream memory = new MemoryStream())
            {
                newPdf.Write(memory);

                foreach (FileStream file in streams.Values)
                {
                    file.Close();
                    file.Dispose();
                }


                byte[] buffer = memory.GetBuffer();

                Response.AddHeader("Content-Disposition", "attachment; filename=out.pdf");

                return File(buffer, "application/pdf");
            }
        }

        public ActionResult Thumbnail(string d, int i)
        {
            using (FileStream upload = new FileStream(Server.MapPath(string.Format("~/Upload/{0}.pdf", d)), FileMode.Open, FileAccess.Read))
            {
                rast.Document pdf = new rast.Document(upload);
                rast.Page page = pdf.Pages[i];

                float resolution = PanelModel.THUMBRES;
                float scale = resolution / 72f;

                int bmpWidth = (int)(scale * page.Width);
                int bmpHeight = (int)(scale * page.Height);

                using (Bitmap bitmap = new Bitmap(bmpWidth, bmpHeight))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (MemoryStream memory = new MemoryStream())
                {
                    graphics.ScaleTransform(scale, scale);
                    page.Draw(graphics);
                    bitmap.Save(memory, ImageFormat.Png);
                    memory.WriteTo(Response.OutputStream);
                }
            }

            return null;
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
