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

            #region Acesso
            UrlConfiguration.AcessoGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarTodos";
            UrlConfiguration.AcessoGet = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarPorId";
            UrlConfiguration.AcessoCreate = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Adicionar";
            UrlConfiguration.AcessoEdit = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Atualizar";
            UrlConfiguration.AcessoDelete = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Deletar";
            #endregion

            #region Emulador
            UrlConfiguration.EmuladorGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/ListarTodos";
            UrlConfiguration.EmuladorGet = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/ListarPorId";
            UrlConfiguration.EmuladorCreate = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Adicionar";
            UrlConfiguration.EmuladorEdit = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Atualizar";
            UrlConfiguration.EmuladorDelete = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Deletar";
            #endregion

            #region Jogo
            UrlConfiguration.JogoGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/ListarTodos";
            UrlConfiguration.JogoGet = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/ListarPorId";
            UrlConfiguration.JogoCreate = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Adicionar";
            UrlConfiguration.JogoEdit = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Atualizar";
            UrlConfiguration.JogoDelete = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Deletar";
            #endregion

            #region Programa
            UrlConfiguration.ProgramaGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/ListarTodos";
            UrlConfiguration.ProgramaGet = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/ListarPorId";
            UrlConfiguration.ProgramaCreate = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Adicionar";
            UrlConfiguration.ProgramaEdit = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Atualizar";
            UrlConfiguration.ProgramaDelete = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Deletar";
            #endregion

            #region Rom
            UrlConfiguration.RomGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/ListarTodos";
            UrlConfiguration.RomGet = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/ListarPorId";
            UrlConfiguration.RomCreate = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Adicionar";
            UrlConfiguration.RomEdit = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Atualizar";
            UrlConfiguration.RomDelete = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Deletar";
            #endregion

            #region Usuario
            UrlConfiguration.UsuarioGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarTodos";
            UrlConfiguration.UsuarioGet = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarPorId";
            UrlConfiguration.UsuarioGetByUserId = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarPorUserId";
            UrlConfiguration.UsuarioCreate = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Adicionar";
            UrlConfiguration.UsuarioEdit = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Atualizar";
            UrlConfiguration.UsuarioDelete = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Deletar";
            #endregion
        }
    }
}
