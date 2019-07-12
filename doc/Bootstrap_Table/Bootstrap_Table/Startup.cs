using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bootstrap_Table.Startup))]
namespace Bootstrap_Table
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
