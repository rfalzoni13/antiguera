using Antiguera.Servicos.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Antiguera.Api.Startup))]

namespace Antiguera.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            AppBuilderConfiguration.ConfigureAuth(app);

            app.UseCors(CorsOptions.AllowAll);

            AppBuilderConfiguration.ActivateAccessToken(app);

            app.UseWebApi(config);
            //AppBuilderConfiguration.ConfigureCors(app);
        }
    }
}