using Antiguera.Administrador.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Configuration;
using System.Diagnostics;

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
            string urlApi = !Debugger.IsAttached ? ConfigurationManager.AppSettings["UrlApiProd"] : ConfigurationManager.AppSettings["UrlApiDev"];

            #region Account
            UrlConfiguration.AccountLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Login";
            UrlConfiguration.AccountLogout = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Logout";
            UrlConfiguration.AccountExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/LoginExterno";
            UrlConfiguration.AccountGetExternalLogins = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/ObterLoginsExternos";
            UrlConfiguration.AccountAddExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AdicionarLoginExterno";
            UrlConfiguration.AccountAddUserExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AdicionarUsuarioLoginExterno";
            UrlConfiguration.AccountRemoveExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/RemoverLoginExterno";
            UrlConfiguration.AccountGetTwoFactorProviders = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/ObterAutenticacaoDoisFatores";
            UrlConfiguration.AccountSendTwoFactorProviderCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoDoisFatores";
            UrlConfiguration.AccountVerifyCodeTwoFactor = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoDoisFatores";
            UrlConfiguration.AccountRegister = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Cadastrar";
            UrlConfiguration.AccountUpdate = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Atualizar";
            UrlConfiguration.AccountDelete = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Apagar";
            UrlConfiguration.AccountSendEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoConfirmacaoEmail";
            UrlConfiguration.AccountSendPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoConfirmacaoTelefone";
            UrlConfiguration.AccountVerifyEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoConfirmacaoEmail";
            UrlConfiguration.AccountVerifyPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoConfirmacaoTelefone";
            UrlConfiguration.AccountChangePassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AlterarSenha";
            UrlConfiguration.AccountForgotPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EsqueciMinhaSenha";
            UrlConfiguration.AccountResetPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/RecuperarSenha";
            #endregion
        }
    }
}
