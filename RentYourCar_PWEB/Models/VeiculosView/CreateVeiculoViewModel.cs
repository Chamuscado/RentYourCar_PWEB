using System.Collections.Generic;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class CreateVeiculoViewModel
    {
        public Veiculo Veiculo { get; set; }
        public IEnumerable<Combustivel> Combustivels { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }

    }
}