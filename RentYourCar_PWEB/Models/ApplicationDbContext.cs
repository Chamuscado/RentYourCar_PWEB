using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RentYourCar_PWEB.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<RentYourCar_PWEB.Models.Veiculo> Veiculoes { get; set; }
    }
}