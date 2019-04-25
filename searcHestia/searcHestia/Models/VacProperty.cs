using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public enum VPType
    {
        [Display(Name = "Entire Home")]
        Entire,
        [Display(Name = "Private Room")]
        Private,
        [Display(Name = "Shared Room")]
        Shared
    }

    public class VacProperty
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Please, provide a title")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Location")]
        [Required]
        public int LocationId { get; set; }

        public int MaxOccupancy { get; set; }

        [DisplayFormat(NullDisplayText = "No type")]
        [Display(Name = "Vacation Property's Type")]
        public VPType VPType { get; set; }

        public Location Location { get; set; }
        public virtual ICollection<Amentity> Amentities { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}