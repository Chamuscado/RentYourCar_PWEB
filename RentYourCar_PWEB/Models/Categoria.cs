﻿namespace RentYourCar_PWEB.Models
{
    public class Categoria
    {
        public byte Id { get; set; }
        public string Nome { get; set; }
        public static readonly byte Max = 5;
    }
}