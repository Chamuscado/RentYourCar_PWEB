using System;
using System.ComponentModel.DataAnnotations;

namespace RentYourCar_PWEB.Models.CustomValidations
{
    public class DateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value ==null)
                return new ValidationResult("A data é inválida. (dd/mm/aaaa)");

            var date = (DateTime) value;

            if (date >= DateTime.Today)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("A data tem de ser igual ou posterior ao dia de hoje.");
        }
    }
}