using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Entidades
{
    public class Emulador
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime DataLancamento { get; set; }

        public string Console { get; set; }

        public string Descricao { get; set; }

        public string UrlArquivo { get; set; }

        public string UrlBoxArt { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual ICollection<Rom> Roms { get; set; }
    }
}
