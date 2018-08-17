using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models
{
    [Table("AvaliacoesFornecedores")]
    public class AvaliacaoFornecedor
    {
        [Key]
        [ForeignKey("Aluguer")]
        public int AluguerId { get; set; }

        public Aluguer Aluguer { get; set; }

        [Required]
        [StringLength(maximumLength: 2048, MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentário")]
        public string Comentario { get; set; }

        [Range(1, 5)]
        public byte Simpatia { get; set; }

        [Range(1, 5)]
        [Display(Name = "Rapidez na Resposta")]
        public byte Rapidez { get; set; }
    }
}