using System;

namespace Antiguera.Administrador.DTOs
{
    public class ProgramaDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public DateTime Lancamento { get; set; }

        public string TipoPrograma { get; set; }

        public string UrlBoxArt { get; set; }

        public string UrlArquivo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}