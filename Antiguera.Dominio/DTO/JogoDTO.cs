using Antiguera.Dominio.DTO.Base;
using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Web;

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

        public string Arquivo { get; set; }

        public string Capa { get; set; }

        public string Tipo { get; set; }

        public string Plataforma { get; set; }

        //Only on DTO
        public string Jogo64 { get; set; }

        public string Capa64 { get; set; }


        public static JogoDTO ConvertToDTO(Jogo jogo)
        {
            return new JogoDTO
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Lancamento = jogo.Lancamento,
                Descricao = jogo.Descricao,
                Arquivo = jogo.Arquivo,
                Capa = jogo.Capa,
                Tipo = jogo.Tipo,
                Genero = jogo.Genero,
                Plataforma = jogo.Plataforma,
                Publisher = jogo.Publisher,
                Developer = jogo.Developer,
                Created = jogo.Created,
                Modified = jogo.Modified,
                Novo = jogo.Novo
            };
        }

        public static List<JogoDTO> ConvertToList(List<Jogo> jogos)
        {
            return jogos.ConvertAll(jogo => new JogoDTO
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Lancamento = jogo.Lancamento,
                Descricao = jogo.Descricao,
                Arquivo = jogo.Arquivo,
                Capa = jogo.Capa,
                Tipo = jogo.Tipo,
                Genero = jogo.Genero,
                Plataforma = jogo.Plataforma,
                Publisher = jogo.Publisher,
                Developer = jogo.Developer,
                Created = jogo.Created,
                Modified = jogo.Modified,
                Novo = jogo.Novo
            });
        }
    }
}
