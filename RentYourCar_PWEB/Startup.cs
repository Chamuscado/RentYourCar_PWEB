using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentYourCar_PWEB.Startup))]
namespace RentYourCar_PWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
