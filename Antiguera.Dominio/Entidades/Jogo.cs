using System;

namespace Antiguera.Dominio.Entidades
{
    public class Jogo
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime Lancamento { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string Genero { get; set; }

        public string UrlBoxArt { get; set; }

        public string UrlArquivo { get; set; }

        public bool? Novo { get; set; }

        public string Plataforma { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}