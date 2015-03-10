using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hazi.WEB.Startup))]
namespace hazi.WEB
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
