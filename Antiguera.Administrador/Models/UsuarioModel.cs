using System;

namespace Antiguera.Administrador.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Genero { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string PathFoto { get; set; }

        public string Telefone { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public string[] Acessos { get; set; }

    }
}