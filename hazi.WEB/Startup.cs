using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hazi.Startup))]
namespace hazi
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
