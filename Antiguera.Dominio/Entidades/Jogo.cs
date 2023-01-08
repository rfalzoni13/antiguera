using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Jogo : EntityBase
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime Lancamento { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public string Plataforma { get; set; }

        public static Jogo ConvertToEntity(JogoDTO jogoDTO)
        {
            return new Jogo
            {
                Id = jogoDTO.Id,
                Nome = jogoDTO.Nome,
                Lancamento = jogoDTO.Lancamento,
                Descricao = jogoDTO.Descricao,
                NomeArquivo = jogoDTO.NomeArquivo,
                HashArquivo = jogoDTO.HashArquivo,
                Genero = jogoDTO.Genero,
                Plataforma = jogoDTO.Plataforma,
                Publisher = jogoDTO.Publisher,
                BoxArt = jogoDTO.BoxArt,
                Developer = jogoDTO.Developer,
                Created = jogoDTO.Created,
                Modified = jogoDTO.Modified,
                Novo = jogoDTO.Novo
            };
        }
    }
}