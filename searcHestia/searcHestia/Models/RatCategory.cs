using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class RatCategory
    {
        public int Id { get; set; }

        [Required]
        public int RatingId { get; set; }

        public string Title { get; set; }

        public float Value { get; set; }

        public Rating Rating { get; set; }
    }
}