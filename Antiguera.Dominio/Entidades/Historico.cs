using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Historico : EntityBase
    {
        public Guid UsuarioId { get; set; }

        public DateTime Data { get; set; }

        public string TipoHistorico { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
