using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetOrganizerWithAngular.Startup))]
namespace AspNetOrganizerWithAngular
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
