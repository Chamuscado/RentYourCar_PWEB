using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class DisponibilidadeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var veiculo = (Veiculo)validationContext.ObjectInstance;

            if (veiculo.InicioDisponibilidade <= veiculo.FimDisponibilidade)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("A data do fim de disponibilidade tem de ser igual ou posterior à data de início.");
        }
    }
}