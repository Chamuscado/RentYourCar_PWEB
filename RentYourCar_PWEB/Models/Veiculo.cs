using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentYourCar_PWEB.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Modelo { get; set; }

        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Marca { get; set; }

        public virtual Combustivel Combustivel { get; set; }
        public byte Lutacao { get; set; }
        public byte NPortas { get; set; }
        public float PrecoDiario { get; set; }
        public float PrecoMensal { get; set; }
        public bool Aprovado { get; set; } = false;
        public virtual Categoria Categoria { get; set; } //pesado, ligeiro, ligeiro de mecadorias, etc

        [StringLength(maximumLength: 2048, MinimumLength = 0)]
        public string CondicoesArrendamento { get; set; } // a ser avaliado

        //public List<Image> Fotografias { get; set; }
    }

}