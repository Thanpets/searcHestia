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
    [Authorize]
    public class ReservationsController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Rating).Include(r => r.VacProperty)
                .Where(r => r.ApplicationUser.UserName.Equals(User.Identity.Name));
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(r => r.VacProperty).Where(r => r.Id == id).FirstOrDefault();
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int vacid)
        {
            var searchdata = TempData["RSearch"] as RSearchViewModel;
            var selectedproperty = db.VacProperties.Where(x=>x.Id == vacid).ToList();

            ViewBag.Id = new SelectList(db.Ratings, "Id", "Id");
            ViewBag.VacPropertyId = new SelectList(selectedproperty, "Id", "Title");

            if (searchdata != null) {
                var reservations = db.Reservations.Include(r => r.VacProperty).FirstOrDefault();

                reservations.OccupantsNum = searchdata.Occupants;
                reservations.StartDate = searchdata.Arrival;
                reservations.EndDate = searchdata.Departure;

                var price = db.Pricings.Include(p => p.VacProperty).Where(p => p.VacPropertyId == vacid &&
                              (searchdata.Arrival >= p.StartDate && searchdata.Arrival <= p.EndDate)).FirstOrDefault();
                if (price != null)
                {
                    reservations.PricePN = price.Price;
                }

                return View(reservations);
            }
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VacPropertyId,StartDate,EndDate,OccupantsNum,CustComments,PricePN")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.ApplicationUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                reservation.RStatus = RStatus.Pending;
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.Id = new SelectList(db.Ratings, "Id", "Id", reservation.Id);
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", reservation.VacPropertyId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Id", reservation.Id);
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", reservation.VacPropertyId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VacPropertyId,StartDate,EndDate,OccupantsNum,DateBooked,CustComments,PricePN,RStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Id", reservation.Id);
            ViewBag.VacPropertyId = new SelectList(db.VacProperties, "Id", "Title", reservation.VacPropertyId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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

        ///// Custom Methods /////
        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult StatusUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(r => r.VacProperty).Where(r => r.Id == id).FirstOrDefault();
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
    }
}
