using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Antiguera.WebApi.Startup))]

namespace Antiguera.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            app.UseCors(CorsOptions.AllowAll);

            ActivateAccessToken(app);

            app.UseWebApi(config);
            //ConfigureCors(app);
        }

        private void ActivateAccessToken(IAppBuilder app)
        {
            var options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //true apenas para ambiente de desenvolvimento
                TokenEndpointPath = new PathString("/api/antiguera/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AccessProviderToken()
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        //private void ConfigureCors(IAppBuilder app)
        //{
        //    var policy = new CorsPolicy();
        //    policy.AllowAnyHeader = true;
        //    policy.Origins.Add('alguma url com porta');
        //    policy.Origins.Add('alguma url com porta');
        //    policy.Methods.Add("GET");
        //    policy.Methods.Add("POST");
        //    var corsOptions = new CorsOptions
        //    {
        //        PolicyProvider = new CorsPolicyProvider
        //        {
        //            PolicyResolver = context => Task.FromResult(policy)
        //        }
        //    };
        //    app.UseCors(corsOptions);
        //}
    }
}
