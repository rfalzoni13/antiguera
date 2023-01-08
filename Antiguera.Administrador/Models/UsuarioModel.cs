using System;

namespace Antiguera.Administrador.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public int AcessoId { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string IdentityUserId { get; set; }

        public string PathFoto { get; set; }

        public bool? Novo { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }


        public virtual AcessoModel Acesso { get; set; }

    }
}