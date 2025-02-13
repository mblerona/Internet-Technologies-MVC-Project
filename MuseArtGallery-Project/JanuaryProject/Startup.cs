using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JanuaryProject.Startup))]
namespace JanuaryProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
