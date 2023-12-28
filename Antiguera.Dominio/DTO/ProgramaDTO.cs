using Antiguera.Dominio.DTO.Base;
using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.DTO
{
    public class ProgramaDTO : BaseDTO
    {
        public ProgramaDTO()
        {
        }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public DateTime Lancamento { get; set; }

        public string Tipo { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }


        public static ProgramaDTO ConvertToDTO(Programa programa)
        {
            return new ProgramaDTO
            {
                Id = programa.Id,
                Nome = programa.Nome,
                Lancamento = programa.Lancamento,
                Descricao = programa.Descricao,
                NomeArquivo = programa.NomeArquivo,
                BoxArt = programa.BoxArt,
                Tipo = programa.Tipo,
                HashArquivo = programa.HashArquivo,
                Publisher = programa.Publisher,
                Developer = programa.Developer,
                Created = programa.Created,
                Modified = programa.Modified,
                Novo = programa.Novo
            };
        }

        public static List<ProgramaDTO> ConvertToList(List<Programa> programas)
        {
            return programas.ConvertAll(programa => new ProgramaDTO
            {
                Id = programa.Id,
                Nome = programa.Nome,
                Lancamento = programa.Lancamento,
                Descricao = programa.Descricao,
                NomeArquivo = programa.NomeArquivo,
                BoxArt = programa.BoxArt,
                Tipo = programa.Tipo,
                HashArquivo = programa.HashArquivo,
                Publisher = programa.Publisher,
                Developer = programa.Developer,
                Created = programa.Created,
                Modified = programa.Modified,
                Novo = programa.Novo
            });
        }
    }
}
