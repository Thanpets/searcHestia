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
    public class VacPropertiesController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: VacProperties
        public ActionResult Index()
        {
            var vacProperties = db.VacProperties.Include(v => v.Location);
            return View(vacProperties.ToList());
        }

        // GET: VacProperties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacProperty vacProperty = db.VacProperties.Find(id);
            if (vacProperty == null)
            {
                return HttpNotFound();
            }
            return View(vacProperty);
        }

        // GET: VacProperties/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Address");
            return View();
        }

        // POST: VacProperties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,LocationId,MaxOccupancy,VPType")] VacProperty vacProperty)
        {
            if (ModelState.IsValid)
            {
                db.VacProperties.Add(vacProperty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Address", vacProperty.LocationId);
            return View(vacProperty);
        }

        // GET: VacProperties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacProperty vacProperty = db.VacProperties.Find(id);
            if (vacProperty == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Address", vacProperty.LocationId);
            return View(vacProperty);
        }

        // POST: VacProperties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,LocationId,MaxOccupancy,VPType")] VacProperty vacProperty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacProperty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Address", vacProperty.LocationId);
            return View(vacProperty);
        }

        // GET: VacProperties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacProperty vacProperty = db.VacProperties.Find(id);
            if (vacProperty == null)
            {
                return HttpNotFound();
            }
            return View(vacProperty);
        }

        // POST: VacProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VacProperty vacProperty = db.VacProperties.Find(id);
            db.VacProperties.Remove(vacProperty);
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
