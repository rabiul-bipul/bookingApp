using System;
using System.ComponentModel.DataAnnotations;
using BookingApp_v1._0.Models;

namespace BookingApp_v1._0.ValidationAttributes
{
    public class BookingDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the value is null
            if (value == null)
            {
                return new ValidationResult("Booking date is required.");
            }

            // Cast the value to DateTime
            DateTime bookingDate = (DateTime)value;

            // Get the BookingInfo instance
            var bookingInfo = validationContext.ObjectInstance as BookingInfo;
            if (bookingInfo == null)
            {
                return new ValidationResult("Invalid booking information.");
            }

            // Access the database context to retrieve the package
            var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            if (dbContext == null)
            {
                return new ValidationResult("Database context is unavailable.");
            }

            var package = dbContext.Packages.Find(bookingInfo.PackageId);
            if (package == null)
            {
                return new ValidationResult("Package not found.");
            }

            // Check if the booking date is after the package expiry date
            if (bookingDate > package.ExpireDate)
            {
                return new ValidationResult("Booking date cannot be after the package expiry date.");
            }

            return ValidationResult.Success;
        }
    }
}
