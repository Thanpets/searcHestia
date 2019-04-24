using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    public enum RStatus
    {
        Pending, Accepted, Declined, Expired, Canceled
    }

    public class Reservation
    {
        public int Id { get; set; }

        //public int UserId { get; set; }

        [Required]
        [Display(Name = "Vacation Property")]
        public int VacPropertyId { get; set; }

        [Display(Name = "Check in")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Check out")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Number of Occupants")]
        public int OccupantsNum { get; set; }

        [Display(Name = "Date of Booking")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateBooked { get; set; }

        [Display(Name = "Comments")]
        public string CustComments { get; set; }

        [Display(Name = "Price Per Night")]
        public double PricePN { get; set; }

        [Display(Name = "Reservation Status")]
        public RStatus RStatus { get; set; }

        public VacProperty VacProperty { get; set; }
        public Rating Rating { get; set; }
    }
}