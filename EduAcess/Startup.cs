using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EduAcess.Startup))]
namespace EduAcess
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
