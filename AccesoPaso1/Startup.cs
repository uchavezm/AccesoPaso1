using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccesoPaso1.Startup))]
namespace AccesoPaso1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
