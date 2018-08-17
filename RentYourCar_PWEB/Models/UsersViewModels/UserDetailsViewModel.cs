using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentYourCar_PWEB.Models.UsersViewModels
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }

        public List<AvaliacaoFornecedor> AvaliacoesFornecedor { get; set; }

        public List<AvaliacaoCliente> AvaliacoesCliente { get; set; }
    }
}