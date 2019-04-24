using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class Pricing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vacation Property")]
        public int VacPropertyId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        [Display(Name = "Occupancy Rate")]
        public double OccRate { get; set; }

        public VacProperty VacProperty { get; set; }
    }
}