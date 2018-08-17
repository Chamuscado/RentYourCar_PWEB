using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RentYourCar_PWEB.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Combustivel> Combustiveis { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Aluguer> Alugueres { get; set; }
        public DbSet<AluguerState> AluguerState { get; set; }
        public DbSet<AvaliacaoVeiculo> AvaliacoesVeiculos  { get; set; }
        public DbSet<AvaliacaoFornecedor> AvaliacoesFornecedores { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}