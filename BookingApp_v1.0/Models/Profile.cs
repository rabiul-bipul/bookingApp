using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp_v1._0.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        // Nullable property with validation
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        // Nullable property with a date range validation
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [CustomValidation(typeof(Profile), nameof(ValidateDateOfBirth))]
        public DateTime? DateOfBirth { get; set; }

        // Nullable property for Contact
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(15, ErrorMessage = "Contact cannot exceed 15 characters.")]
        public string Contact { get; set; }

        // Nullable property for Address
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; }

        // Nullable property for ProfilePicture
        [Url(ErrorMessage = "Please enter a valid URL for the profile picture.")]
        public string ProfilePicture { get; set; }

        // Foreign key to AspNetUsers table
        
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        // Navigation property to AspNetUsers
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Custom validator for DateOfBirth (to ensure it is in the past)
        public static ValidationResult ValidateDateOfBirth(DateTime? dateOfBirth, ValidationContext context)
        {
            if (!dateOfBirth.HasValue || dateOfBirth.Value > DateTime.Now)
            {
                return new ValidationResult("Date of Birth must be a valid date in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
