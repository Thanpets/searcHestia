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
            var vpresult = db.VacProperties.Include(l => l.Location).Where(v =>
               String.IsNullOrEmpty(search.location) || v.Location.City.Name.Contains(search.location) ||
               v.Location.City.Region.Name.Contains(search.location));
            TempData["RSearch"] = search;

            return View(vpresult.ToList());
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