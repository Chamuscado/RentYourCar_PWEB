﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models
{
    [Table("Alugueres")]
    public class Aluguer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public ApplicationUser Cliente { get; set; }

        [Required]
        [Display(Name = "Veículo")]
        public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo Veiculo { get; set; }

        [Required]
        [Display(Name = "Início")]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fim { get; set; }
    }
}