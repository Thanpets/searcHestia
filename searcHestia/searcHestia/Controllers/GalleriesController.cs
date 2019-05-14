using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using searcHestia.Models;

namespace searcHestia.Controllers
{
    public class GalleriesController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: Galleries
        public ActionResult Index(int vacid)
        {
            var galleries = db.Galleries.Include(g => g.VacProperty)
                .Where(g => g.VacPropertyId == vacid);

            TempData["VacId"] = vacid;
            return View(galleries.ToList());
        }

        // GET: Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Galleries/Create
        public ActionResult Create()
        {
            var vacid = Convert.ToInt32(TempData["VacId"]);
            var selectedproperty = db.VacProperties.Where(x => x.Id == vacid).ToList();
            ViewBag.VacPropertyId = new SelectList(selectedproperty, "Id", "Title");
            //ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title");
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VacPropertyId,Details")] Gallery gallery, HttpPostedFileBase imagefile)
        {
            string subPath = "";
            string fileName = "";

            var someFile = imagefile;
            try
            {
                if (someFile != null && someFile.ContentLength > 0)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "JPG", "JPEG", "PNG" };
                    var fileExt = System.IO.Path.GetExtension(someFile.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("imagemsg", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        return View("Create", gallery);
                    }
                    if (someFile.ContentLength > 481280)
                    {
                        ModelState.AddModelError("photo", "The size of the file should not exceed 470 KB");
                        return View("Create", gallery);
                    }
                    string ServerPath = Server.MapPath("~");
                    subPath = @"/PhotoGallery/"; // @"D:\Temp\";

                    subPath += DateTime.Now.Year.ToString();
                    string fullPath = ServerPath + subPath;

                    bool exists = System.IO.Directory.Exists(fullPath);
                    if (!exists)
                        System.IO.Directory.CreateDirectory(fullPath);


                    subPath += @"/" + DateTime.Now.Month.ToString("d2");
                    fullPath = ServerPath + subPath;
                    exists = System.IO.Directory.Exists(fullPath);

                    if (!exists)
                        System.IO.Directory.CreateDirectory(fullPath);

                    fileName = gallery.VacPropertyId.ToString() + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + fileExt;//Path.GetFileName(file.FileName);

                    someFile.SaveAs(Path.Combine(fullPath, fileName));
                }
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("imagemsg", ex.Message);
                return View("Create");
            }



            if (ModelState.IsValid)
            {
                gallery.Path = subPath + @"/";
                gallery.Name = fileName;// "fff";
                db.Galleries.Add(gallery);
                db.SaveChanges();
                return RedirectToAction("Index", new { vacid = gallery.VacPropertyId });
            }

            return View(gallery);
        }

        // GET: Galleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", gallery.VacPropertyId);
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VacPropertyId,Name,Path,Details")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { vacid = gallery.VacPropertyId });
            }
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", gallery.VacPropertyId);
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Galleries.Find(id);
            db.Galleries.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Index", new { vacid = gallery.VacPropertyId });
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
