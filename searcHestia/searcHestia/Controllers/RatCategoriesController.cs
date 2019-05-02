using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using searcHestia.Models;

namespace searcHestia.Controllers
{
    public class RatCategoriesController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: RatCategories
        public ActionResult Index()
        {
            var ratCategories = db.RatCategories.Include(r => r.Rating);
            return View(ratCategories.ToList());
        }

        // GET: RatCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatCategory ratCategory = db.RatCategories.Find(id);
            if (ratCategory == null)
            {
                return HttpNotFound();
            }
            return View(ratCategory);
        }

        // GET: RatCategories/Create
        public ActionResult Create()
        {
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "Id");
            return View();
        }

        // POST: RatCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RatingId,Title,Value")] RatCategory ratCategory)
        {
            if (ModelState.IsValid)
            {
                db.RatCategories.Add(ratCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "Id", ratCategory.RatingId);
            return View(ratCategory);
        }

        // GET: RatCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatCategory ratCategory = db.RatCategories.Find(id);
            if (ratCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "Id", ratCategory.RatingId);
            return View(ratCategory);
        }

        // POST: RatCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RatingId,Title,Value")] RatCategory ratCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "Id", ratCategory.RatingId);
            return View(ratCategory);
        }

        // GET: RatCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatCategory ratCategory = db.RatCategories.Find(id);
            if (ratCategory == null)
            {
                return HttpNotFound();
            }
            return View(ratCategory);
        }

        // POST: RatCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatCategory ratCategory = db.RatCategories.Find(id);
            db.RatCategories.Remove(ratCategory);
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
