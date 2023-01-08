using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models
{
    public class EmuladorModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Console { get; set; }

        public string Descricao { get; set; }

        public string hashArquivo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual ICollection<RomModel> Roms { get; set; }
    }
}