using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

[assembly: OwinStartup(typeof(Antiguera.WebApi.Startup))]

namespace Antiguera.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;

            ConfigureOAuthTokenGeneration(app);

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

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
                TokenEndpointPath = new PathString("/api/antiguera/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AccessProviderToken(),
                RefreshTokenProvider = new AccessRefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
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
