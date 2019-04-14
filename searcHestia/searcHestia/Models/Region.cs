using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public class Region
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please, provide a name")]
        public string Name { get; set; }
    }
}