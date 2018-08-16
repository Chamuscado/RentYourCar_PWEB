using System.Collections.Generic;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class DetailsVeiculoViewModel
    {
        public Veiculo Veiculo { get; set; }
        public string Combustivel { get; set; }
        public string Categoria { get; set; }
        public string Proprietario { get; set; }
        public List<string> imagesPaths { get; set; }
        public List<AvaliacaoVeiculo> Avaliacoes { get; set; }
    }
}