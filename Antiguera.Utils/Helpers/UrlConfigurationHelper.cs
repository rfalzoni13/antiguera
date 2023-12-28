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

        #region Acesso
        public static string AcessoGetAll;
        public static string AcessoGet;
        public static string AcessoCreate;
        public static string AcessoEdit;
        public static string AcessoDelete;
        #endregion

        #region Emulador
        public static string EmuladorGetAll;
        public static string EmuladorGet;
        public static string EmuladorCreate;
        public static string EmuladorEdit;
        public static string EmuladorDelete;
        #endregion

        #region Jogo
        public static string JogoGetAll;
        public static string JogoGet;
        public static string JogoCreate;
        public static string JogoEdit;
        public static string JogoDelete;
        #endregion

        #region Programa
        public static string ProgramaGetAll;
        public static string ProgramaGet;
        public static string ProgramaCreate;
        public static string ProgramaEdit;
        public static string ProgramaDelete;
        #endregion

        #region Rom
        public static string RomGetAll;
        public static string RomGet;
        public static string RomCreate;
        public static string RomEdit;
        public static string RomDelete;
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

            #region Acesso
            AcessoGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarTodos";
            AcessoGet = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/ListarPorId";
            AcessoCreate = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Inserir";
            AcessoEdit = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Atualizar";
            AcessoDelete = $"{urlApi}/{ConfigurationManager.AppSettings["AcessoUrl"]}/Deletar";
            #endregion

            #region Emulador
            EmuladorGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/ListarTodos";
            EmuladorGet = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/ListarPorId";
            EmuladorCreate = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Inserir";
            EmuladorEdit = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Atualizar";
            EmuladorDelete = $"{urlApi}/{ConfigurationManager.AppSettings["EmuladorUrl"]}/Deletar";
            #endregion

            #region Jogo
            JogoGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/ListarTodos";
            JogoGet = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/ListarPorId";
            JogoCreate = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Inserir";
            JogoEdit = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Atualizar";
            JogoDelete = $"{urlApi}/{ConfigurationManager.AppSettings["JogoUrl"]}/Deletar";
            #endregion

            #region Programa
            ProgramaGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/ListarTodos";
            ProgramaGet = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/ListarPorId";
            ProgramaCreate = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Inserir";
            ProgramaEdit = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Atualizar";
            ProgramaDelete = $"{urlApi}/{ConfigurationManager.AppSettings["ProgramaUrl"]}/Deletar";
            #endregion

            #region Rom
            RomGetAll = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/ListarTodos";
            RomGet = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/ListarPorId";
            RomCreate = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Inserir";
            RomEdit = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Atualizar";
            RomDelete = $"{urlApi}/{ConfigurationManager.AppSettings["RomUrl"]}/Deletar";
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
