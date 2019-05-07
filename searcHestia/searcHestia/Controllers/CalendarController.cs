using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using searcHestia.Models;
using searcHestia.ViewModels;
using System.Data.Entity;

namespace searcHestia.Controllers
{
    public class CalendarController : ApiController
    {
        private SearchestiaContext db = new SearchestiaContext();

        [HttpGet]
        public HttpResponseMessage Get()
        {
            //
            // FullCalendar will keep passing the start and end values in as you navigate through the calendar pages
            //  You should therefore use these days to determine what events you should return . Ie
            //  i.e.     var events = db.Events.Where(event => event.Start > start && event.End < end);

            //
            // Below is dummy data to show us how the event object can be serialized 
            //

            /*
               var events = new List<CalendarEvent>();

               events.Add(new CalendarEvent
               {
                  title = "",
                  start = DateTime.Now.ToString("yyyy-MM-dd"),
                  end = DateTime.Now.ToString("yyyy-MM-dd")
               });
            */

            var events = db.Reservations.Include(r => r.VacProperty)
               .Where(r => r.VacProperty.ApplicationUser.UserName == User.Identity.Name).AsEnumerable()
               .Select(r => new CalendarEvent { title = r.VacProperty.Title, start = r.StartDate.ToString("yyyy-MM-dd"), end = r.EndDate.ToString("yyyy-MM-dd") }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, events, Request.GetConfiguration());

        }
    }
}
