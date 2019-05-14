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
    public class PricingsController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: Pricings
        public ActionResult Index(int vacid)
        {
            var pricings = db.Pricings.Include(p => p.VacProperty).Where(a => a.VacPropertyId == vacid);
            TempData["VacId"] = vacid;
            return View(pricings.ToList());
        }

        // GET: Pricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pricing pricing = db.Pricings.Find(id);
            if (pricing == null)
            {
                return HttpNotFound();
            }
            return View(pricing);
        }

        // GET: Pricings/Create
        public ActionResult Create()
        {
            var vacid = Convert.ToInt32(TempData["VacId"]);
            var selectedproperty = db.VacProperties.Where(x => x.Id == vacid).ToList();

            ViewBag.VacPropertyId = new SelectList(selectedproperty, "Id", "Title");
            //ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title");
            return View();
        }

        // POST: Pricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VacPropertyId,StartDate,EndDate,Price,Description,OccRate")] Pricing pricing)
        {
            if (ModelState.IsValid)
            {
                db.Pricings.Add(pricing);
                db.SaveChanges();
                return RedirectToAction("Index", new { vacid = pricing.VacPropertyId });
            }

            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", pricing.VacPropertyId);
            return View(pricing);
        }

        // GET: Pricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pricing pricing = db.Pricings.Find(id);
            if (pricing == null)
            {
                return HttpNotFound();
            }

            ViewBag.VacPropertyId = new SelectList(db.VacProperties.Where(x => x.Id == pricing.VacPropertyId).ToList(), "Id", "Title");
            //ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", pricing.VacPropertyId);
            return View(pricing);
        }

        // POST: Pricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VacPropertyId,StartDate,EndDate,Price,Description,OccRate")] Pricing pricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { vacid = pricing.VacPropertyId });
            }
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", pricing.VacPropertyId);
            return View(pricing);
        }

        // GET: Pricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pricing pricing = db.Pricings.Find(id);
            if (pricing == null)
            {
                return HttpNotFound();
            }
            return View(pricing);
        }

        // POST: Pricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pricing pricing = db.Pricings.Find(id);
            db.Pricings.Remove(pricing);
            db.SaveChanges();
            return RedirectToAction("Index", new { vacid = pricing.VacPropertyId });
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
