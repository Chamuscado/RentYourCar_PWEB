using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentYourCar_PWEB.Models
{
    [Table("Alugueres")]
    public class Aluguer
    {
        public int Id { get; set; }

        [Required] [Display(Name = "Cliente")] public string ClienteId { get; set; }

        [ForeignKey("ClienteId")] public ApplicationUser Cliente { get; set; }

        [Required] [Display(Name = "Veículo")] public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")] public Veiculo Veiculo { get; set; }

        [Required] [Display(Name = "Início")] public DateTime Inicio { get; set; }

        [Required] public DateTime Fim { get; set; }

        [ForeignKey("AluguerState_id")] public AluguerState AluguerState { get; set; }


        [Display(Name = "Estado", Description = "O estado em que se encontra o Aluguer")]
        public byte AluguerState_id { get; set; } = AluguerState.Pendente;


        public AvaliacaoVeiculo AvaliacaoVeiculo { get; set; }
    }
}