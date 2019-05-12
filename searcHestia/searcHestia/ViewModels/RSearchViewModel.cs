using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.ViewModels
{
    public class RSearchViewModel
    {
        public string location { get; set; }

        [Required(ErrorMessage ="Please provide arrival date")]
        [Display(Name = "Arrival-Date")]
        [DataType(DataType.Date)]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "Please provide departure date")]
        [Display(Name = "Departure-Date")]
        [DataType(DataType.Date)]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "Please provide occupants number")]
        [Display(Name = "Number of Occupants")]
        [Range(1,10, ErrorMessage = "Only positive number allowed")]
        public int Occupants { get; set; }

        /*IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (Departure < Arrival)
            {
                yield return new ValidationResult("Check-out must be greater than Check-in");
            }
        }
        */
    }

}