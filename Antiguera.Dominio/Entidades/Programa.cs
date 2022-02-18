using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Programa : EntityBase
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public DateTime Lancamento { get; set; }
        
        public string Tipo { get; set; }

        public string BoxArt { get; set; }

        public string nomeArquivo { get; set; }

        public string hashArquivo { get; set; }
    }
}