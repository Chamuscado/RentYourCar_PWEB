using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RentYourCar_PWEB.Models.AlugueresViewModels;

namespace RentYourCar_PWEB.Models.CustomValidations
{
    public class PeriodoAluguerValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var aluguer = (CreateAluguerViewModel) validationContext.ObjectInstance;

            if (aluguer.Inicio <= aluguer.Fim)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("A data do fim de aluguer tem de ser igual ou posterior à data de início.");
        }
    }
}