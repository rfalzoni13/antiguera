using Antiguera.Dominio.DTO.Base;
using System;

namespace Antiguera.Dominio.DTO
{
    public class JogoDTO : BaseDTO
    {
        public JogoDTO()
        {
        }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime Lancamento { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string nomeArquivo { get; set; }

        public string hashArquivo { get; set; }

        public string Plataforma { get; set; }
    }
}
