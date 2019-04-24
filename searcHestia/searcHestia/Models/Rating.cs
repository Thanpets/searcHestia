using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{

    public class Rating
    {
        public int Id { get; set; }

        public float Overall { get; set; }

        public Reservation Reservation { get; set; }
        public virtual ICollection<RatCategory> RatCategories { get; set; }
    }
}