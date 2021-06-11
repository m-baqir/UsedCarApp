using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarApp.Startup))]
namespace UsedCarApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
