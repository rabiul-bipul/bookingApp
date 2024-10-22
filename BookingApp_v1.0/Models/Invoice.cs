using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v1._0.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int ProfileId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        // Navigation properties
        public virtual Profile Profile { get; set; }
        public virtual Package Package { get; set; }
    }
}
