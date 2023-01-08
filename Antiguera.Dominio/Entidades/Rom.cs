using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Rom : EntityBase
    {
        public Guid EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public static Rom ConvertToEntity(RomDTO romDTO)
        {
            return new Rom
            {
                Id = romDTO.Id,
                EmuladorId = romDTO.EmuladorId,
                Nome = romDTO.Nome,
                Lancamento = romDTO.Lancamento,
                Descricao = romDTO.Descricao,
                NomeArquivo = romDTO.NomeArquivo,
                HashArquivo = romDTO.HashArquivo,
                Genero = romDTO.Genero,
                BoxArt = romDTO.BoxArt,
                Created = romDTO.Created,
                Modified = romDTO.Modified,
                Novo = romDTO.Novo
            };
        }
    }
}
