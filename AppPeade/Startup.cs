using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppPeade.Startup))]
namespace AppPeade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
