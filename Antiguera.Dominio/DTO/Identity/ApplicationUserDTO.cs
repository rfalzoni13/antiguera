using System;

namespace Antiguera.Dominio.DTO.Identity
{
    public class ApplicationUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }
        public DateTime? DateBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class ApplicationUserRegisterDTO
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Genero { get; set; }
        public Guid IdAcesso { get; set; }
        public string PathFoto { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool AcceptTerms { get; set; }
    }

    public class ChangePasswordBindingDTO
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
