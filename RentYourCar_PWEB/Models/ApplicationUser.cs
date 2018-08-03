using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RentYourCar_PWEB.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Morada { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public bool Aprovado { get; set; }
        public virtual ICollection<Veiculo> Veiculos { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}