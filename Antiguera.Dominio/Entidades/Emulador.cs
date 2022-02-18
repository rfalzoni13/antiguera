using Antiguera.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Entidades
{
    public class Emulador : EntityBase
    {
        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Console { get; set; }

        public string Descricao { get; set; }

        public string nomeArquivo { get; set; }

        public string hashArquivo { get; set; }

        public virtual ICollection<Rom> Roms { get; set; }
    }
}
