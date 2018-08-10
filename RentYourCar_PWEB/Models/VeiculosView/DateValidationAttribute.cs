using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class DateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime) value;

            if (date >= DateTime.Today)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("A data tem de ser igual ou posterior ao dia de hoje.");
        }
    }
}