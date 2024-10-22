using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v1._0.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.Today)
                {
                    return new ValidationResult("Expire Date must be a future date.");
                }
            }
            return ValidationResult.Success;
        }
    }
}