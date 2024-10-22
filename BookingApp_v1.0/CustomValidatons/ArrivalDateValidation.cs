using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v1._0.Validations
{
    public class ArrivalDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date <= DateTime.Now)
                {
                    return new ValidationResult("Arrival Date must be a future date.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
