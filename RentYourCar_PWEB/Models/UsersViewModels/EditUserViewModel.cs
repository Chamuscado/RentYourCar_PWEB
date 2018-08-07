using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models.UsersViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O campo {0} tem de ter no mínimo {2} caracteres.", MinimumLength = 10)]
        public string Nome { get; set; }

        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "Indique um número com 9 dígitos.")]
        public string Telefone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O campo {0} tem de ter no mínimo {2} caracteres.", MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public string Morada { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}