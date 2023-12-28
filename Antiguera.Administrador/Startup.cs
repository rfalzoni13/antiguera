using Antiguera.Utils.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Antiguera.Administrador
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });

            UrlConfigurationHelper.SetUrlList();
        }
    }
}
