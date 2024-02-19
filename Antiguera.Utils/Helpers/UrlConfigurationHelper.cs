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
        public static string AccountGetExternalLogins;
        public static string AccountAddExternalLogin;
        public static string AccountAddUserExternalLogin;
        public static string AccountRemoveExternalLogin;
        public static string AccountChangePassword;
        public static string AccountForgotPassword;
        public static string AccountResetPassword;
        #endregion

        #region IdentityUtility
        public static string IdentityUtilityGetTwoFactorProviders;
        public static string IdentityUtilitySendTwoFactorProviderCode;
        public static string IdentityUtilityVerifyCodeTwoFactor;
        public static string IdentityUtilitySendEmailConfirmationCode;
        public static string IdentityUtilitySendPhoneConfirmationCode;
        public static string IdentityUtilityVerifyEmailConfirmationCode;
        public static string IdentityUtilityVerifyPhoneConfirmationCode;
        #endregion

        #region Acesso
        public static string AcessoGetAllNames;
        public static string AcessoGetAll;
        public static string AcessoGet;
        public static string AcessoCreate;
        public static string AcessoEdit;
        public static string AcessoDelete;
        #endregion

        #region Usuario
        public static string UsuarioGetAll;
        public static string UsuarioGet;
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
            AccountChangePassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/AlterarSenha";
            AccountForgotPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/EsqueciMinhaSenha";
            AccountResetPassword = $"{urlApi}/{ConfigurationManager.AppSettings["AccountUrl"]}/RecuperarSenha";
            #endregion

            #region IdentityUtility
            IdentityUtilityGetTwoFactorProviders = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/ObterAutenticacaoDoisFatores";
            IdentityUtilitySendTwoFactorProviderCode = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/EnviarCodigoDoisFatores";
            IdentityUtilityVerifyCodeTwoFactor = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/VerificarCodigoDoisFatores";
            IdentityUtilitySendEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/EnviarCodigoConfirmacaoEmail";
            IdentityUtilitySendPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/EnviarCodigoConfirmacaoTelefone";
            IdentityUtilityVerifyEmailConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/VerificarCodigoConfirmacaoEmail";
            IdentityUtilityVerifyPhoneConfirmationCode = $"{urlApi}/{ConfigurationManager.AppSettings["IdentityUtilityUrl"]}/VerificarCodigoConfirmacaoTelefone";
            #endregion

            #region Acesso
            AcessoGetAllNames = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarTodosNomes";
            AcessoGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarTodos";
            AcessoGet = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarPorId";
            AcessoCreate = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Inserir";
            AcessoEdit = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Atualizar";
            AcessoDelete = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Deletar";
            #endregion

            #region Usuario
            UsuarioGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarTodos";
            UsuarioGet = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/ListarPorId";
            UsuarioCreate = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Inserir";
            UsuarioEdit = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Atualizar";
            UsuarioDelete = $"{urlApi}/{ConfigurationManager.AppSettings["UsuarioUrl"]}/Deletar";
            #endregion
        }

    }
}
