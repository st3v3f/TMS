using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tms.Web.Startup))]
namespace Tms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
