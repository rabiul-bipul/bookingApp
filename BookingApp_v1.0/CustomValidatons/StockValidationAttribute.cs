/*using BookingApp_v1._0.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookingApp_v1._0.Validations
{
    public class StockValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
             
            // Check if the value is an integer and not null
            if (value is int quantity)
            {
                // Get the BookingInfo instance
                var bookingInfo = validationContext.ObjectInstance as BookingInfo;

                if (bookingInfo == null)
                {
                    return new ValidationResult("Invalid booking information.");
                }

                var packageId = bookingInfo.PackageId;

                // Access the database context to get the stock quantity
                var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

                if (dbContext == null)
                {
                    return new ValidationResult("Database context is not available.");
                }

                var package = dbContext.Packages.Find(packageId);

                if (package == null)
                {
                    return new ValidationResult("Package not found.");
                }

                if (quantity > package.Stock) // Assuming Stock is a property in your Package model
                {
                    return new ValidationResult($"Quantity cannot exceed available stock of {package.Stock}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
*/