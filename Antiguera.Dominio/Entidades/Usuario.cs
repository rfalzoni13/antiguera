using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Usuario : EntityBase
    {
        public int AcessoId { get; set; }

        public string Nome { get; set; }

        public string IdentityUserId { get; set; }

        public string pathFoto { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public virtual Acesso Acesso { get; set; }
    }
}