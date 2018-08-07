using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using RentYourCar_PWEB.Models.VeiculosView;

namespace RentYourCar_PWEB.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        public int Id { get; set; }


        [Display(Name = "Modelo", Description = "Modelo do veículo")]
        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Modelo { get; set; }

        [Display(Name = "Marca", Description = "Marca do veículo")]
        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 2)]
        public string Marca { get; set; }

        [Display(Name = "Lutação", Description = "Lutação do veículo")]
        [Required]
        public byte Lutacao { get; set; }

        [Display(Name = "Portas", Description = "Número de Portas do veículo")]
        [Required]
        public byte NPortas { get; set; }

        [Display(Name = "Matrícula", Description = "Matrícula do veículo")]
        [MatriculaValidationAttribute]
        // [Required(ErrorMessage = "Insira a Matricula Formato: \"AA-00-00\", \"00-AA-00\" ou \"00-00-AA\" ")]
        [StringLength(maximumLength: 8, MinimumLength = 8)]
        public string Matricula { get; set; }

        [Display(Name = "Combustível", Description = "Combustível do veículo")]
        [Required(ErrorMessage = "Escolha o Combustivel")]
        public byte Combustivel_id { get; set; }

        [Display(Name = "Categoria", Description = "Categoria do veículo")]
        [Required(ErrorMessage = "Escolha a Categoria")]
        public byte Categoria_id { get; set; } //pesado, ligeiro, ligeiro de mecadorias, etc

        [Display(Name = "Preço Diario", Description = "Valor cobrado por cada dia de utilização do veículo")]
        [Required]
        public float PrecoDiario { get; set; }

        [Display(Name = "Preço Mensal", Description = "Valor cobrado por cada mês de utilização do veículo")]
        [Required]
        public float PrecoMensal { get; set; }

        [Display(Name = "Aprovado", Description = "O veículo deve ser aprovado por um administrador")]
        public bool Aprovado { get; set; } = false;

        [Display(Name = "Condicoes de Arrendamento")]
        [StringLength(maximumLength: 2048, MinimumLength = 0)]
        public string CondicoesArrendamento { get; set; } // a ser avaliado

        public string UserId { get; set; }


        //public List<Image> Fotografias { get; set; }
    }
}