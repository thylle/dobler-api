using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoblerAPI.Startup))]
namespace DoblerAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
