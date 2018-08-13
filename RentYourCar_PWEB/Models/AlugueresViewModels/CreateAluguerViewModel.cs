using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RentYourCar_PWEB.Models.CustomValidations;

namespace RentYourCar_PWEB.Models.AlugueresViewModels
{
    public class CreateAluguerViewModel
    {
        [Required]
        public int VeiculoId { get; set; }

        public Veiculo Veiculo { get; set; }

        [Required]
        public string ClienteId { get; set; }

        [Display(Name = "Início")]
        [Required]
        [DateValidationAttribute]
        public DateTime Inicio { get; set; }

        [Required]
        [DateValidationAttribute]
        [PeriodoAluguerValidationAttribute]
        public DateTime Fim { get; set; }
    }
}