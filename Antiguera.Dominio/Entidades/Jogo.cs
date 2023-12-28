using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;

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

        public string Arquivo { get; set; }

        public string Capa { get; set; }

        public string Tipo { get; set; }

        public string Plataforma { get; set; }

        public static Jogo ConvertToEntity(JogoDTO jogoDTO)
        {
            return new Jogo
            {
                Id = jogoDTO.Id,
                Nome = jogoDTO.Nome,
                Lancamento = jogoDTO.Lancamento,
                Descricao = jogoDTO.Descricao,
                Arquivo = jogoDTO.Arquivo,
                Capa = jogoDTO.Capa,
                Tipo = jogoDTO.Tipo,
                Genero = jogoDTO.Genero,
                Plataforma = jogoDTO.Plataforma,
                Publisher = jogoDTO.Publisher,
                Developer = jogoDTO.Developer,
                Created = jogoDTO.Created,
                Modified = jogoDTO.Modified,
                Novo = jogoDTO.Novo
            };
        }

        public static List<Jogo> ConvertToList(List<JogoDTO> jogosDTO)
        {
            return jogosDTO.ConvertAll(jogoDTO => new Jogo
            {
                Id = jogoDTO.Id,
                Nome = jogoDTO.Nome,
                Lancamento = jogoDTO.Lancamento,
                Descricao = jogoDTO.Descricao,
                Arquivo = jogoDTO.Arquivo,
                Capa = jogoDTO.Capa,
                Tipo = jogoDTO.Tipo,
                Genero = jogoDTO.Genero,
                Plataforma = jogoDTO.Plataforma,
                Publisher = jogoDTO.Publisher,
                Developer = jogoDTO.Developer,
                Created = jogoDTO.Created,
                Modified = jogoDTO.Modified,
                Novo = jogoDTO.Novo
            });
        }
    }
}