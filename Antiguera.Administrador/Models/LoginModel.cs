using System;
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
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }
        public Guid IdAcesso { get; set; }
        public string PathFoto { get; set; }
        public bool AcceptTerms { get; set; }
    }

    public class SendCodeModel
    {
        public List<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string SelectedProvider { get; set; }
    }

    public class VerifyCodeModel
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ReturnVerifyCodeModel
    {
        public string ReturnUrl { get; set; }
    }

    public class GenerateTokenEmailModel
    {
        public string UserId { get; set; }
        public string Url { get; set; }
    }

    public class GenerateTokenPhoneModel
    {
        public string UserId { get; set; }
        public string Phone { get; set; }
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

    public class ConfirmPhoneCodeModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
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

    public class ExternalLoginModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
    }

    public class AddExternalLoginBindingModel
    {
        public string ExternalAccessToken { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        public string Email { get; set; }
    }

    //public class RegistrarModel
    //{
    //    public bool RememberMe { get; set; }
    //    public string Email { get; set; }
    //    public string Nome { get; set; }
    //    public string Password { get; set; }
    //}
}