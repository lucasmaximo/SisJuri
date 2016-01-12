using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sisjuri.Startup))]
namespace Sisjuri
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
