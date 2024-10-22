using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingApp_v1._0.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [Display(Name ="Location")]
        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
        public string LocationName { get; set; }

        
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        [Url(ErrorMessage ="Not a Url")]
        public string Description { get; set; }
    }
}