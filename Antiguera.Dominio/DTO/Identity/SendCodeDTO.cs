using System.Collections.Generic;

namespace Antiguera.Dominio.DTO.Identity
{
    public class SendCodeDTO
    {
        public string UserId { get; set; }
        public string SelectedProvider { get; set; }
    }

    public class VerifyCodeDTO
    {
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public string Code { get; set; }
    }

    public class ReturnVerifyCodeDTO
    {
        public string ReturnUrl { get; set; }
    }

    public class GenerateTokenEmailDTO
    {
        public string UserId { get; set; }
        public string Url { get; set; }
    }

    public class ConfirmEmailCodeDTO
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string CallBackUrl { get; set; }
    }

    public class GenerateTokenPhoneDTO
    {
        public string UserId { get; set; }
        public string Phone { get; set; }
    }

    public class ConfirmPhoneCodeDTO
    {
        public string UserId { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
    }


    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }

    public class IdentityResultCodeDTO
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
