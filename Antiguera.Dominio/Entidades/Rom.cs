using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Rom : EntityBase
    {
        public int EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string nomeArquivo { get; set; }

        public string hashArquivo { get; set; }
    }
}
