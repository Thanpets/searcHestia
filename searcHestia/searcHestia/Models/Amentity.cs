using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class Amentity
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please, provide a title")]
        public string Title { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<VacProperty> VacProperties { get; set; }

    }
}