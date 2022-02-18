using Antiguera.Dominio.DTO.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.DTO
{
    public class EmuladorDTO : BaseDTO
    {
        public EmuladorDTO()
        {
        }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Console { get; set; }

        public string Descricao { get; set; }

        public string hashArquivo { get; set; }

        public virtual ICollection<RomDTO> Roms { get; set; }
    }
}
