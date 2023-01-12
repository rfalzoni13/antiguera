using System;

namespace Antiguera.Administrador.Models
{
    public class RomModel
    {
        public Guid Id { get; set; }

        public Guid EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}