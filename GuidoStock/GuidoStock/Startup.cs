using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GuidoStock.Startup))]
namespace GuidoStock
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
