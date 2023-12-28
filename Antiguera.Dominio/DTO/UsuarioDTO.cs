using Antiguera.Dominio.DTO.Base;
using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.DTO
{
    public class UsuarioDTO : BaseDTO
    {
        public UsuarioDTO()
        {
        }
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Genero { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string PathFoto { get; set; }

        public string Telefone { get; set; }

        public string[] Acessos { get; set; }
    }
}
