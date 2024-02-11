using System.Configuration;
using System.Diagnostics;

namespace Antiguera.Utils.Helpers
{
    public class UrlConfigurationHelper
    {
        #region Account
        public static string AccountLogin;
        public static string AccountLogout;
        public static string AccountExternalLogin;
        public static string AccountGetTwoFactorProviders;
        public static string AccountSendTwoFactorProviderCode;
        public static string AccountVerifyCodeTwoFactor;
        public static string AccountGetExternalLogins;
        public static string AccountAddExternalLogin;
        public static string AccountAddUserExternalLogin;
        public static string AccountRemoveExternalLogin;
        public static string AccountChangePassword;
        public static string AccountForgotPassword;
        public static string AccountResetPassword;
        public static string AccountRegister;
        public static string AccountUpdate;
        public static string AccountDelete;
        public static string AccountSendEmailConfirmationCode;
        public static string AccountSendPhoneConfirmationCode;
        public static string AccountVerifyEmailConfirmationCode;
        public static string AccountVerifyPhoneConfirmationCode;
        #endregion

        #region Usuario
        public static string UsuarioGetAll;
        public static string UsuarioGet;
        public static string UsuarioGetByUserId;
        public static string UsuarioCreate;
        public static string UsuarioEdit;
        public static string UsuarioDelete;
        #endregion


        public static void SetUrlList()
        {
            string urlApi = !Debugger.IsAttached ? ConfigurationManager.AppSettings["UrlApiProd"] : ConfigurationManager.AppSettings["UrlApiDev"];

            #region Account
            AccountLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Login";
            AccountLogout = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Logout";
            AccountExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/LoginExterno";
            AccountGetExternalLogins = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/ObterLoginsExternos";
            AccountAddExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AdicionarLoginExterno";
            AccountAddUserExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AdicionarUsuarioLoginExterno";
            AccountRemoveExternalLogin = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/RemoverLoginExterno";
            AccountGetTwoFactorProviders = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/ObterAutenticacaoDoisFatores";
            AccountSendTwoFactorProviderCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoDoisFatores";
            AccountVerifyCodeTwoFactor = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoDoisFatores";
            AccountRegister = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Cadastrar";
            AccountUpdate = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Atualizar";
            AccountDelete = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/Apagar";
            AccountSendEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoConfirmacaoEmail";
            AccountSendPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EnviarCodigoConfirmacaoTelefone";
            AccountVerifyEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoConfirmacaoEmail";
            AccountVerifyPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/VerificarCodigoConfirmacaoTelefone";
            AccountChangePassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AlterarSenha";
            AccountForgotPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EsqueciMinhaSenha";
            AccountResetPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/RecuperarSenha";
            #endregion

            #region Usuario
            UsuarioGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarTodos";
            UsuarioGet = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarPorId";
            UsuarioGetByUserId = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarPorUserId";
            UsuarioCreate = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Inserir";
            UsuarioEdit = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Atualizar";
            UsuarioDelete = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Deletar";
            #endregion
        }

    }
}
