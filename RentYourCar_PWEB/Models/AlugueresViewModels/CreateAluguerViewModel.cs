using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models.AlugueresViewModels
{
    public class CreateAluguerViewModel
    {
        [Required]
        public int VeiculoId { get; set; }

        [Required]
        public string ClienteId { get; set; }

        [Display(Name = "Início")]
        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fim { get; set; }
    }
}