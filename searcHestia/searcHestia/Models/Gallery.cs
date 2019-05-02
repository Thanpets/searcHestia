using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vacation Property")]
        public int VacPropertyId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Path { get; set; }

        public string Details { get; set; }

        public VacProperty VacProperty { get; set; }
    }
}