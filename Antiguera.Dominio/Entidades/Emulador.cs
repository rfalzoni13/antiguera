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

        public static Emulador ConvertToEntity(EmuladorDTO emuladorDTO)
        {
            return new Emulador
            {
                Id = emuladorDTO.Id,
                Nome = emuladorDTO.Nome,
                Lancamento = emuladorDTO.Lancamento,
                Console = emuladorDTO.Console,
                Descricao = emuladorDTO.Descricao,
                NomeArquivo = emuladorDTO.Nome,
                HashArquivo = emuladorDTO.HashArquivo,
                Created = emuladorDTO.Created,
                Modified = emuladorDTO.Modified,
                Novo = emuladorDTO.Novo,
                Roms = emuladorDTO.Roms.ToList().ConvertAll(r => new Rom { 
                    Id = r.Id,
                    EmuladorId = r.EmuladorId,
                    Nome = r.Nome,
                    Descricao = r.Descricao,
                    BoxArt = r.BoxArt,
                    Lancamento = r.Lancamento,
                    NomeArquivo = r.Nome,
                    HashArquivo = r.HashArquivo,
                    Genero = r.Genero,
                    Novo = r.Novo,
                    Created = r.Created,
                    Modified = r.Modified
                })
            };
        }
    }
}
