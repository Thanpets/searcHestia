using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class ApplicationUser : IdentityUser
    {
        //add your custom properties which have not included in IdentityUser before
        //public string MyExtraProperty { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<VacProperty> VacProperties { get; set; }
    }
}