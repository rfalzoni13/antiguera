using System;

namespace Antiguera.Api.Models
{
    public class ApplicationUserRegisterModel
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

    public class AddExternalLoginBindingModel
    {
        public string ExternalAccessToken { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        public string Email { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}