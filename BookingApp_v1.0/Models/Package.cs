using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingApp_v1._0.ValidationAttributes;

namespace BookingApp_v1._0.Models
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Package Name is required.")]
        [StringLength(100, ErrorMessage = "Package Name cannot exceed 100 characters.")]
        public string PackageName { get; set; }

        [Required(ErrorMessage = "Details are required.")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Expire Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Expire Date")]
        [FutureDate]
        public DateTime ExpireDate { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Sold count is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Sold count must be a non-negative integer.")]
        public int Sold { get; set; }

        [Required(ErrorMessage = "Stock count is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }

        // Foreign key for Hotel
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        // Foreign key for Location
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}
