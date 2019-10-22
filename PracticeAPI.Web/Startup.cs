using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PracticeAPI.Web.Startup))]

namespace PracticeAPI.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
