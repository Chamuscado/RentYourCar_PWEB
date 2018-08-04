using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RentYourCar_PWEB.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Modelo { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Marca { get; set; }

        //[Required]
        public virtual Combustivel Combustivel { get; set; }
        [Required] public byte Lutacao { get; set; }
        [Required] public byte NPortas { get; set; }
        [Required] public float PrecoDiario { get; set; }
        [Required] public float PrecoMensal { get; set; }
        public bool Aprovado { get; set; } = false;
        //[Required]
        public virtual Categoria Categoria { get; set; } //pesado, ligeiro, ligeiro de mecadorias, etc

        [StringLength(maximumLength: 2048, MinimumLength = 0)]
        public string CondicoesArrendamento { get; set; } // a ser avaliado

        //public List<Image> Fotografias { get; set; }
    }
}