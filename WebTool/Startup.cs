using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTool.Startup))]
namespace WebTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
