using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;

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

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public static Programa ConvertToEntity(ProgramaDTO programaDTO)
        {
            return new Programa
            {
                Id = programaDTO.Id,
                Nome = programaDTO.Nome,
                Lancamento = programaDTO.Lancamento,
                Descricao = programaDTO.Descricao,
                NomeArquivo = programaDTO.NomeArquivo,
                HashArquivo = programaDTO.HashArquivo,
                Tipo = programaDTO.Tipo,
                Publisher = programaDTO.Publisher,
                BoxArt = programaDTO.BoxArt,
                Developer = programaDTO.Developer,
                Created = programaDTO.Created,
                Modified = programaDTO.Modified,
                Novo = programaDTO.Novo
            };
        }

        public static List<Programa> ConvertToList(List<ProgramaDTO> programasDTO)
        {
            return programasDTO.ConvertAll(programaDTO => new Programa
            {
                Id = programaDTO.Id,
                Nome = programaDTO.Nome,
                Lancamento = programaDTO.Lancamento,
                Descricao = programaDTO.Descricao,
                NomeArquivo = programaDTO.NomeArquivo,
                BoxArt = programaDTO.BoxArt,
                Tipo = programaDTO.Tipo,
                HashArquivo = programaDTO.HashArquivo,
                Publisher = programaDTO.Publisher,
                Developer = programaDTO.Developer,
                Created = programaDTO.Created,
                Modified = programaDTO.Modified,
                Novo = programaDTO.Novo
            });
        }
    }
}