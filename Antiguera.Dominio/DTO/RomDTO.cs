using Antiguera.Dominio.DTO.Base;
using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.DTO
{
    public class RomDTO : BaseDTO
    {
        public RomDTO()
        {
        }

        public Guid EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string BoxArt { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public static RomDTO ConvertToDTO(Rom rom)
        {
            return new RomDTO
            {
                Id = rom.Id,
                EmuladorId = rom.EmuladorId,
                Nome = rom.Nome,
                Lancamento = rom.Lancamento,
                Descricao = rom.Descricao,
                NomeArquivo = rom.NomeArquivo,
                HashArquivo = rom.HashArquivo,
                Genero = rom.Genero,
                BoxArt = rom.BoxArt,
                Created = rom.Created,
                Modified = rom.Modified,
                Novo = rom.Novo
            };
        }

        public static List<RomDTO> ConvertToList(List<Rom> roms)
        {
            return roms.ConvertAll(rom => new RomDTO
            {
                Id = rom.Id,
                EmuladorId = rom.EmuladorId,
                Nome = rom.Nome,
                Lancamento = rom.Lancamento,
                Descricao = rom.Descricao,
                NomeArquivo = rom.NomeArquivo,
                HashArquivo = rom.HashArquivo,
                Genero = rom.Genero,
                BoxArt = rom.BoxArt,
                Created = rom.Created,
                Modified = rom.Modified,
                Novo = rom.Novo
            });
        }
    }
}
