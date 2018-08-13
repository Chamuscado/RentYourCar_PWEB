using System.ComponentModel.DataAnnotations;

namespace RentYourCar_PWEB.Models.CustomValidations
{
    public class MatriculaValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var veiculo = (Veiculo) validationContext.ObjectInstance;
            if (Matricula.Matricula.IsValid(veiculo.Matricula)) //TODO -> invocar DLL
                return ValidationResult.Success;
            else
            {
                return new ValidationResult("Formatos Permitidos: "+Matricula.Matricula.GetValidMatriculas());
            }
        }
    }
}