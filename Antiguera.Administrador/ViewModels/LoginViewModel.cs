using System.ComponentModel.DataAnnotations;

namespace Antiguera.Administrador.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class PasswordRecoveryViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}