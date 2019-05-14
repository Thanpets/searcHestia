using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using searcHestia.Models;

namespace searcHestia.ViewModels
{
    public class CreatePropertyViewModel
    {
        public VacProperty VacProperty { get; set; }

        public Location Location { get; set; }

        public IEnumerable<SelectListItem> CitiesSelectListItems { get; set; }
    }
}