using BookingApp_v1._0.ValidationAttributes;
using BookingApp_v1._0.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp_v1._0.Models
{
    public class BookingInfo
    {
        [Key]
        public int BookingID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("Profile")]
        [Required(ErrorMessage = "Profile ID is required.")]
        public int ProfileId { get; set; }

        [ForeignKey("Package")]
        [Required(ErrorMessage = "Package ID is required.")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than 0.")]
        public double TotalPrice { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
       
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Arrival date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        public string PaymentMethod { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Package Package { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
