using System.Collections.Generic;
using System.Web.Mvc;

namespace Antiguera.Administrador.Models
{
    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegistrarModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AcceptTerms { get; set; }
    }

    public class SendCodeModel
    {
        public List<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string SelectedProvider { get; set; }
    }

    public class ExternalLoginConfirmationModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string IdProvider { get; set; }
        public string ProviderName { get; set; }
        public bool AcceptTerms { get; set; }
    }

    public class VerifyCodeModel
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }



    public class ReturnCodeStatusModel
    {
        public ESignInStatusCode Status { get; set; }
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ConfirmEmailCodeModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class IdentityResultCodeModel
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
        public string CallBackUrl { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }


    public enum ESignInStatusCode
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }

    //public class RegistrarModel
    //{
    //    public bool RememberMe { get; set; }
    //    public string Email { get; set; }
    //    public string Nome { get; set; }
    //    public string Password { get; set; }
    //}
}