using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp_v1._0.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Hotel Name is required.")]
        [StringLength(100, ErrorMessage = "Hotel Name cannot exceed 100 characters.")]
        public string HotelName { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(255, ErrorMessage = "Hotel Website cannot exceed 255 characters.")]
        public string HotelWebsite { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}
