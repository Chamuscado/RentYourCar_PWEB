using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RentYourCar_PWEB.Models;

[assembly: OwinStartupAttribute(typeof(RentYourCar_PWEB.Startup))]

namespace RentYourCar_PWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            //Criar Role de "Admin" e um utilizador para este papel
            if (!roleManager.RoleExists(RoleNames.Admin))
            {
                //Criar o Role "Admin"
                var role = new IdentityRole
                {
                    Name = RoleNames.Admin
                };
                roleManager.Create(role);

                //Criar o utilizador Admin
                var user = new ApplicationUser
                {
                    Nome = "Admin",
                    Email = "admin@rentyourcar.com",
                    UserName = "admin@rentyourcar.com",
                    Aprovado = true
                };

                string password = "_Password123";

                var createResult = userManager.Create(user, password);

                //Adicionar o Role "Admin" ao utilizador por defeito
                if (createResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, RoleNames.Admin);
                }
            }

            //Criar o Role "Profissional"
            if (!roleManager.RoleExists(RoleNames.Profissional))
            {
                var role = new IdentityRole
                {
                    Name = RoleNames.Profissional
                };
                roleManager.Create(role);
            }

            //Criar o Role "Particular"   
            if (!roleManager.RoleExists(RoleNames.Particular))
            {
                var role = new IdentityRole
                {
                    Name = RoleNames.Particular
                };
                roleManager.Create(role);
            }
        }
    }
}