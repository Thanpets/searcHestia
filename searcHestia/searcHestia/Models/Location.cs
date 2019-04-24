using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int RegionId { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(6, MinimumLength = 5, ErrorMessage = "Post Code must be at least 5 characters.")]
        public string ZIPCode { get; set; }

        public double? LatCoordinate { get; set; }

        public double? LngCoordinate { get; set; }

        public Region Region { get; set; }

    }
}