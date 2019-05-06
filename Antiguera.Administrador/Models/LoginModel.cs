using System.ComponentModel.DataAnnotations;

namespace Antiguera.Administrador.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class PasswordRecoveryModel
    {
        [Required]
        public string Email { get; set; }
    }
}