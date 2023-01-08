using System;

namespace Antiguera.Administrador.Models
{
    public class ProgramaModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public DateTime Lancamento { get; set; }

        public string Tipo { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}