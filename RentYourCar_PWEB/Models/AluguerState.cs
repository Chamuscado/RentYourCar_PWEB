using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentYourCar_PWEB.Models
{
    [Table("AluguerState")]
    public class AluguerState
    {
        public byte Id { get; set; }
        public string Nome { get; set; }

        public static readonly byte Max = 5;

        public static readonly byte Aceite = 1;
        public static readonly byte EmCurso = 2;
        public static readonly byte Pendente = 3;
        public static readonly byte Rejeitado = 4;
        public static readonly byte Concluído = 5;
    }
}