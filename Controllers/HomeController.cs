using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using kit = TallComponents.PDF;
using rast = TallComponents.PDF.Rasterizer;

using Matrix.Models;
using System.Web.Routing;

namespace Matrix.Controllers
{
    public class HomeController : Controller
    {
        private MatrixEntities db = new MatrixEntities();
        public ActionResult Index()
        {
            ViewBag.NbDoc = db.CSP_DOC.Count();
            ViewBag.docs = new SelectList(db.CSP_DOC, "Id", "CodeDoc");
            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle");
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle");
            ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN");
            return View();
        }

        public ActionResult Panel()
        {
            ViewBag.IdC = TempData["IdC"];
            ViewBag.Cs = TempData["Cs"];
            ViewBag.Url = TempData["IdClient"];

            return PartialView("_Panel", new PanelModel());

        }

        [HttpPost]
        public ActionResult Upload(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files[0];
            Guid guid = Guid.NewGuid();
            kit.Document pdf = new kit.Document(file.InputStream);
            file.SaveAs(Server.MapPath(string.Format("~/Upload/{0}.pdf", guid)));

             String Idc = form["IdClient"].ToString();
             String Csp = form["CodeCSP"].ToString();
             String Url = file.FileName;

            ViewBag.Idc = TempData["Idc"];
            ViewBag.Cs = TempData["Cs"];
            ViewBag.Url = TempData["Url"];
            var cSP_DOC =  db.CSP_DOC.Where(c => c.CodeCSP == Csp & c.CodeDoc != "Glob").ToList();
            
           

            return PartialView("_Panel", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf, Idc = Idc, Cs = Csp, Url=Url, cSP_DOC= cSP_DOC});
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
            using (FileStream file = new FileStream(Server.MapPath(string.Format("~/Upload/{0}.pdf", d)), FileMode.Open, FileAccess.Read))
            {
                rast.Document pdf = new rast.Document(file);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientDoc clientDoc)
        {
            HttpPostedFileBase upload = Request.Files[0];

            string csp = clientDoc.CodeCSP;
            TempData["Idc"] = clientDoc.IdClient;
            TempData["Cs"] = clientDoc.CodeCSP;
            ViewBag.Url = TempData["Url"];


            if (ModelState.IsValid)
            {

            //    string path = Path.Combine(Server.MapPath("~/Uploads"), ViewBag.Url);
                clientDoc.UrlDoc = ViewBag.FileName;
                db.ClientDoc.Add(clientDoc);
                db.SaveChanges();


                ViewBag.NbDoc = db.CSP_DOC.Count();
                ViewBag.docs = new SelectList(db.CSP_DOC, "Id", "CodeDoc");
                ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", clientDoc.CodeCSP);
                ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", clientDoc.CodeDoc);
                ViewBag.IdDoc = new SelectList(db.DonneesCIN, "IdDoc", "NOCIN", clientDoc.IdDoc);
                TempData["Url"] = clientDoc.UrlDoc;

                //  return PartialView("_Details");
                //  return RedirectToAction("fileupload", new RouteValueDictionary(
                //      new { controller = "Home", action = "Upload", upload = upload }));

                return RedirectToAction("Details", new RouteValueDictionary(
                      new { controller = "Depouillement", action = "Details", csp = csp }));

                //   return PartialView("_Panel", new PanelModel() { DocumentGuid = guid.ToString(), Document = pdf });
            }

            ViewBag.CodeCSP = new SelectList(db.CSP, "Code", "Libelle", clientDoc.CodeCSP);
            ViewBag.CodeDoc = new SelectList(db.Documents, "Code", "Libelle", clientDoc.CodeDoc);
            return View(clientDoc);
        }


        [HttpGet]
        public ActionResult Details(string csp)
        {
            ViewBag.Idc = TempData["Idc"];
            ViewBag.Cs = TempData["Cs"];
            ViewBag.Url = TempData["Url"];
            var cSP_DOC = db.CSP_DOC.Where(c => c.CodeCSP == csp & c.CodeDoc != "Glob").ToList();
            ViewBag.data = cSP_DOC;

            return Content("cSP_DOC.ToList()");
        }
    }
}