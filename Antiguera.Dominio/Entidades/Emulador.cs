using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiguera.Dominio.Entidades
{
    public class Emulador : EntityBase
    {
        public string Nome { get; set; }

        public DateTime Lancamento { get; set; }

        public string Console { get; set; }

        public string Descricao { get; set; }

        public string NomeArquivo { get; set; }

        public string HashArquivo { get; set; }

        public virtual ICollection<Rom> Roms { get; set; }

        public static EmuladorDTO ConvertToEntity(Emulador emulador)
        {
            return new EmuladorDTO
            {
                Id = emulador.Id,
                Nome = emulador.Nome,
                Lancamento = emulador.Lancamento,
                Console = emulador.Console,
                Descricao = emulador.Descricao,
                NomeArquivo = emulador.NomeArquivo,
                HashArquivo = emulador.HashArquivo,
                Created = emulador.Created,
                Modified = emulador.Modified,
                Novo = emulador.Novo,
                Roms = emulador.Roms != null ? emulador.Roms.ToList().ConvertAll(rom => new RomDTO
                {
                    Id = rom.Id,
                    Nome = rom.Nome,
                    BoxArt = rom.BoxArt,
                    Descricao = rom.Descricao,
                    EmuladorId = rom.EmuladorId,
                    Lancamento = rom.Lancamento,
                    HashArquivo = rom.HashArquivo,
                    NomeArquivo = rom.NomeArquivo,
                    Genero = rom.Genero,
                    Created = rom.Created,
                    Modified = rom.Modified,
                    Novo = rom.Novo
                }) : null
            };
        }

        public static List<EmuladorDTO> ConvertToList(List<Emulador> emuladors)
        {
            return emuladors.ConvertAll(emulador => new EmuladorDTO
            {
                Id = emulador.Id,
                Nome = emulador.Nome,
                Lancamento = emulador.Lancamento,
                Console = emulador.Console,
                Descricao = emulador.Descricao,
                NomeArquivo = emulador.NomeArquivo,
                HashArquivo = emulador.HashArquivo,
                Created = emulador.Created,
                Modified = emulador.Modified,
                Novo = emulador.Novo,
                Roms = emulador.Roms != null ? emulador.Roms.ToList().ConvertAll(rom => new RomDTO
                {
                    Id = rom.Id,
                    Nome = rom.Nome,
                    BoxArt = rom.BoxArt,
                    Descricao = rom.Descricao,
                    EmuladorId = rom.EmuladorId,
                    Lancamento = rom.Lancamento,
                    HashArquivo = rom.HashArquivo,
                    NomeArquivo = rom.NomeArquivo,
                    Genero = rom.Genero,
                    Created = rom.Created,
                    Modified = rom.Modified,
                    Novo = rom.Novo
                }) : null
            });
        }
    }
}
