using Antiguera.Dominio.DTO.Identity;
using System;
using System.Collections.Generic;

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
        public string PathFoto { get; set; }
        public bool AcceptTerms { get; set; }
        public List<string> Acessos { get; set; }

        public static ApplicationUserRegisterDTO ConvertToDTO(ApplicationUserRegisterModel model)
        {
            return new ApplicationUserRegisterDTO
            {
                ID = model.ID,
                Nome = model.Nome,
                Email = model.Email,
                Login = model.Login,
                PathFoto = model.PathFoto,
                Genero = model.Genero,
                DataNascimento = model.DataNascimento.Date,
                Acessos = model.Acessos.ToArray(),
                Senha = model.Senha,
                AcceptTerms = model.AcceptTerms
            };
        }
    }

    public class AddExternalLoginBindingModel
    {
        public string ExternalAccessToken { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        public string Email { get; set; }
    }

    public class ExternalLoginModel
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