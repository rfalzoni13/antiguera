using Antiguera.Administrador.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Configuration;

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

            SetUrlList();
        }

        private void SetUrlList()
        {
            string urlApi = ConfigurationManager.AppSettings["AmbienteApi"] == "true" ?
            ConfigurationManager.AppSettings["UrlApiProd"] :
            ConfigurationManager.AppSettings["UrlApiDev"];

            UrlConfiguration.Login = urlApi + ConfigurationManager.AppSettings["LoginUrl"];
            UrlConfiguration.Logout = urlApi + ConfigurationManager.AppSettings["LogoutUrl"];
            UrlConfiguration.SendCode = urlApi + ConfigurationManager.AppSettings["SendCodeUrl"];
            UrlConfiguration.VerifyCode = urlApi + ConfigurationManager.AppSettings["VerifyCodeUrl"];
            UrlConfiguration.GetSmsProviders = urlApi + ConfigurationManager.AppSettings["SmsProvidersListUrl"];
            UrlConfiguration.ConfirmEmail = urlApi + ConfigurationManager.AppSettings["ConfirmEmailUrl"];
            UrlConfiguration.ForgotPassword = urlApi + ConfigurationManager.AppSettings["ForgotPasswordUrl"];
            UrlConfiguration.ResetPassword = urlApi + ConfigurationManager.AppSettings["ResetPasswordUrl"];
            UrlConfiguration.Register = urlApi + ConfigurationManager.AppSettings["RegisterUrl"];
            UrlConfiguration.Update = urlApi + ConfigurationManager.AppSettings["UpdateUrl"];
            UrlConfiguration.Delete = urlApi + ConfigurationManager.AppSettings["DeleteUrl"];
        }
    }
}
