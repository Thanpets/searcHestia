using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using searcHestia.Models;
using searcHestia.ViewModels;

namespace searcHestia.Controllers
{
    public class RatingsController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: Ratings
        public ActionResult Index()
        {
            var rating = db.Ratings.Include(r => r.RatCategories);

            return View(rating.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        [Authorize]
        // GET: Ratings/Create
        public ActionResult Create(int resid)
        {
            var rating = new Rating();
            rating.RatCategories = new List<RatCategory>();
            RateCategories(rating);
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Overall")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Overall")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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

        //---------------------------------------------------------
        // CUSTOM METHODS
        //---------------------------------------------------------

        private void RateCategories(Rating rating)
        {
            //var allCategories = db.RatCategories;
            List<string> allCategories = new List<string>(new string[] { "Accuracy", "Communication", "Cleanliness", "Location" });
            var ratingCategories = new HashSet<int>(rating.RatCategories.Select(c => c.Id));
            var viewModel = new List<SelectedCategoriesStars>();
            for (int i = 0; i < allCategories.Count; i++)
            {
                viewModel.Add(new SelectedCategoriesStars
                {
                    CategoryID = i,
                    Title = allCategories[i],
                    Picked = ratingCategories.Contains(i)
                });
            }
            ViewBag.RatCategories = viewModel;
        }
    }
}
