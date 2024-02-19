using System;

namespace Antiguera.Administrador.Models.Identity
{
    public class ApplicationUserModel
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }
        public string PathFoto { get; set; }
        public bool AcceptTerms { get; set; }
    }
}