using Antiguera.Dominio.DTO.Base;
using System;

namespace Antiguera.Dominio.DTO
{
    public class HistoricoDTO : BaseDTO
    {
        public HistoricoDTO()
        {
        }

        public Guid UsuarioId { get; set; }

        public DateTime Data { get; set; }

        public string TipoHistorico { get; set; }

        public virtual UsuarioDTO Usuario { get; set; }
    }
}
