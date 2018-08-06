using System.ComponentModel.DataAnnotations;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class MatriculaValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (true) //TODO -> invocar DLL
                return ValidationResult.Success;
            else
            {
                return new ValidationResult("Formatos Primitidos: ");//TODO -> invocar Formatos
            }
        }
    }
}