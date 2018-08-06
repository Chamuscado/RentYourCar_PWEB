using System.ComponentModel.DataAnnotations.Schema;

namespace RentYourCar_PWEB.Models
{
    [Table("Combustiveis")]
    public class Combustivel
    {
        public byte Id { get; set; }
        public string Nome { get; set; }

        public static readonly byte Max = 5;
    }

}