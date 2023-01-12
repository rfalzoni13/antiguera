using System.Configuration;

namespace Antiguera.Administrador.Helpers
{
    public static class UrlConfiguration
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
    }
}