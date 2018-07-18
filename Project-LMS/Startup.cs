using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_LMS.Startup))]
namespace Project_LMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
