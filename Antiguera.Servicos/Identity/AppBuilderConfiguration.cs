using Antiguera.Infra.Data.Contexto;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;

namespace Antiguera.Servicos.Identity
{
    public static class AppBuilderConfiguration
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        }

        //public static void ConfigureCookieToken(IAppBuilder app)
        //{
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions()
        //    {
        //        AuthenticationType = "ApplicationCookie",
        //        LoginPath = new PathString("/Account/Login"),

        //    });
        //}

        public static void ActivateAccessToken(IAppBuilder app)
        {
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //true apenas para ambiente de desenvolvimento
                TokenEndpointPath = new PathString("/Api/Account/Login"),
                AuthorizeEndpointPath = new PathString("/Api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                RefreshTokenProvider = new AccessRefreshTokenProvider()
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UseOAuthAuthorizationServer(OAuthOptions);

            app.UseOAuthBearerTokens(OAuthOptions);


            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            //app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Remova comentários das linhas a seguir para habilitar o login com provedores de login de terceiros
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            app.UseFacebookAuthentication(
                appId: "1699681303742860",
                appSecret: "1e2d0379d4f20856d12097b8399f1130");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        //public static void ConfigureCors(IAppBuilder app)
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
