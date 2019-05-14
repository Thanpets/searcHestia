using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using searcHestia.Models;
using searcHestia.ViewModels;

namespace searcHestia.Controllers
{
    public class HomeController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();

        //GET
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[ActionName("SearchResult")]
        public ActionResult Search(RSearchViewModel search)
        {
            var vpresult = db.VacProperties.Include(l => l.Location.City).Where(v =>
               (String.IsNullOrEmpty(search.location) || v.Location.City.Name.Contains(search.location) ||
               v.Location.City.Region.Name.Contains(search.location)) &&

                (search.Occupants == 0 || search.Occupants <= v.MaxOccupancy) &&

                (db.Availabilities
                .Where(o => (v.Id == o.VacPropertyId) && (search.Arrival >= o.StartDate && search.Arrival <= o.EndDate) && (search.Departure >= o.StartDate && search.Departure <= o.EndDate))
                .Select(o => o.VacPropertyId).ToList().Contains(v.Id)) &&
                !(db.Reservations
                .Where(r => (v.Id == r.VacPropertyId) && ((search.Arrival >= r.StartDate && search.Arrival <= r.EndDate) || (search.Departure >= r.StartDate && search.Departure <= r.EndDate)) && (search.Arrival != r.EndDate))
                .Select(s => s.VacPropertyId).ToList().Contains(v.Id))).Select(vac => vac).Distinct().ToList();

            foreach (var item in vpresult)
            {
                item.Galleries = db.Galleries.Include(v => v.VacProperty).Where(v => v.VacPropertyId == item.Id).ToList();
            }
            TempData["RSearch"] = search;

            return View(vpresult);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        ///////////////

        public ActionResult Dashboard()
        {
            List<Reservation> reservations = null;

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Owner"))
                {
                    reservations = db.Reservations.Include(r => r.VacProperty.ApplicationUser)
                       .Where(r => r.VacProperty.ApplicationUser.UserName.Equals(User.Identity.Name)
                       && r.RStatus == RStatus.Pending).ToList();
                }
                else
                {
                    reservations = db.Reservations.Include(r => r.VacProperty)
                        .Where(r => r.ApplicationUser.UserName.Equals(User.Identity.Name)).ToList();
                }
                return View(reservations);
            }

            return RedirectToAction("Index");
        }
    }
}