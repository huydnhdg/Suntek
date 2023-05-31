using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Suntek.Startup))]
namespace Suntek
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
