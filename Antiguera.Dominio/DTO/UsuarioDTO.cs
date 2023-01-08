using Antiguera.Dominio.DTO.Base;
using System;

namespace Antiguera.Dominio.DTO
{
    public class UsuarioDTO : BaseDTO
    {
        public UsuarioDTO()
        {
        }

        public Guid AcessoId { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Genero { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string IdentityUserId { get; set; }

        public string PathFoto { get; set; }

        public string Telefone { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public virtual AcessoDTO Acesso { get; set; }
    }
}
