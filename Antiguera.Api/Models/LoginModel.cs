using System.ComponentModel.DataAnnotations;

namespace Antiguera.Api.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class SendCodeModel
    {
        public string UserId { get; set; }
        public string SelectedProvider { get; set; }
    }

    public class VerifyCodeModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class GenerateTokenEmailModel
    {
        public string UserId { get; set; }
        public string Url { get; set; }
    }

    public class ConfirmEmailCodeModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class GenerateTokenPhoneModel
    {
        public string UserId { get; set; }
        public string Phone { get; set; }
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
}