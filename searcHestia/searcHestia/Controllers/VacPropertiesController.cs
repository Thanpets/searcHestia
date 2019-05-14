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
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace searcHestia.Controllers
{
    [Authorize]
    public class VacPropertiesController : Controller
    {
        private SearchestiaContext db = new SearchestiaContext();
        //private readonly UserManager<ApplicationUser> _userManager;

        // GET: VacProperties
        public ActionResult Index()
        {
            var vacProperties = db.VacProperties.Include(v => v.Location);
            return View(vacProperties.ToList());
        }

        [AllowAnonymous]
        // GET: VacProperties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacProperty vacProperty = db.VacProperties.Include(l => l.Location.City.Region).Where(v => v.Id == id).FirstOrDefault();

            if (vacProperty == null)
            {
                return HttpNotFound();
            }

            vacProperty.Galleries = db.Galleries.Include(v => v.VacProperty).Where(v => v.VacPropertyId == id).ToList();

            return View(vacProperty);
        }

        //[Authorize(Roles = "Owner")]
        // GET: VacProperties/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Address");

            var vproperty = new VacProperty();
            vproperty.Amenities = new List<Amenity>();
            PopulateSelectedAmenities(vproperty);
            var vmcreateproperty = new CreatePropertyViewModel();
            vmcreateproperty.CitiesSelectListItems = new SelectList(db.Cities, "Id", "Name");
            return View(vmcreateproperty);
        }

        // POST: VacProperties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePropertyViewModel createproperty, double? lat, double? lng, string[] selectedAmenities)
        {
            if (selectedAmenities != null)
            {
                createproperty.VacProperty.Amenities = new List<Amenity>();
                foreach (var amenity in selectedAmenities)
                {
                    var amenityToAdd = db.Amenities.Find(int.Parse(amenity));
                    createproperty.VacProperty.Amenities.Add(amenityToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                Location location = db.Locations
                    .Include(l => l.City)
                    .Where(l => l.CityId == createproperty.Location.CityId)
                    .Where(l => l.ZIPCode.Equals(createproperty.Location.ZIPCode))
                    .Where(l => l.Address.Equals(createproperty.Location.Address))
                    .FirstOrDefault();

                if (location == null)
                {
                    createproperty.Location.LatCoordinate = lat;
                    createproperty.Location.LngCoordinate = lng;
                    db.Locations.Add(createproperty.Location);
                }
                else
                {
                    createproperty.Location = location;
                }

                //var currentUserId = User.Identity.GetUserId();
                createproperty.VacProperty.ApplicationUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                createproperty.VacProperty.LocationId = createproperty.Location.Id;
                db.VacProperties.Add(createproperty.VacProperty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateSelectedAmenities(createproperty.VacProperty);
            ViewBag.CityId = new SelectList(db.Regions, "Id", "Name", createproperty.VacProperty.Location.CityId);
            return View(createproperty.VacProperty);
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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,LocationId,MaxOccupancy,VPType,PricePN")] VacProperty vacProperty)
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

        //---------------------------------------------------------
        // CUSTOM METHODS
        //---------------------------------------------------------

        private void PopulateSelectedAmenities(VacProperty vproperty)
        {
            var allAmenities = db.Amenities;
            var vpropertyAmenities = new HashSet<int>(vproperty.Amenities.Select(c => c.Id));  //a set that contains no duplicate elements
            var viewModel = new List<SelectedAmenityData>();
            foreach (var amenity in allAmenities)
            {
                viewModel.Add(new SelectedAmenityData
                {
                    AmenityID = amenity.Id,
                    Title = amenity.Title,
                    Picked = vpropertyAmenities.Contains(amenity.Id)
                });
            }
            ViewBag.Amenities = viewModel;
        }

        private void UpdateVacPropertyAmenities(string[] selectedAmenities, VacProperty vpropertyToUpdate)
        {
            if (selectedAmenities == null)
            {
                vpropertyToUpdate.Amenities = new List<Amenity>();
                return;
            }

            var selectedAmenitiesHS = new HashSet<string>(selectedAmenities);
            var vpropertyAmenities = new HashSet<int>
                (vpropertyToUpdate.Amenities.Select(c => c.Id));
            foreach (var amenity in db.Amenities)
            {
                if (selectedAmenitiesHS.Contains(amenity.Id.ToString()))
                {
                    if (!vpropertyAmenities.Contains(amenity.Id))
                    {
                        vpropertyToUpdate.Amenities.Add(amenity);
                    }
                }
                else
                {
                    if (vpropertyAmenities.Contains(amenity.Id))
                    {
                        vpropertyToUpdate.Amenities.Remove(amenity);
                    }
                }
            }
        }

        public ActionResult GetLocations(int id)
        {
            var locations = db.VacProperties.Include(v => v.Location)
                .Where(v => v.Id == id).Select(v=> new { v.Title, v.Location.LatCoordinate, v.Location.LngCoordinate }).FirstOrDefault();
           
            return Json(locations, JsonRequestBehavior.AllowGet);
        }
    }
}
