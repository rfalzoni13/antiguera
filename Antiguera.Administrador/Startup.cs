using Antiguera.Administrador.Context;
using Antiguera.Administrador.Models.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Antiguera.Administrador.Startup))]

namespace Antiguera.Administrador
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(Contexto.Create);
            app.CreatePerOwinContext<UsuarioAppManager>(UsuarioAppManager.Create);
            app.CreatePerOwinContext<SignInAppManager>(SignInAppManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
                CookieName = "Antiguera",
                CookiePath = "/"
            });
        }
    }
}
