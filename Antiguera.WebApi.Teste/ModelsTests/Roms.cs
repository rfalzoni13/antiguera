using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Roms
    {
        public List<Rom> ListaRoms
        {
            get
            {
                var lista = new List<Rom>();

                lista.Add(new Rom()
                {
                    Id = 1,
                    EmuladorId = 1,
                    Nome = "Super Mario World",
                    Genero = "Plataforma",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "Super Mario World originalmente chamado no Japão de Super Mario Bros. 4, é um jogo de plataforma desenvolvido e publicado pela Nintendo como um título que acompanhava o console Super Nintendo Entertainment System.",
                    DataLancamento = new DateTime(1990, 11, 21),
                    Emulador = new Emulador
                    {
                        Id = 1,
                        Nome = "ZSnes",
                        Console = "Super Nintendo",
                        Created = DateTime.Now,
                        Novo = true,
                        Descricao = "Emulador de Super Nintendo",
                        DataLancamento = new DateTime(1997, 1, 1)
                    }
                });

                lista.Add(new Rom()
                {
                    Id = 2,
                    EmuladorId = 2,
                    Nome = "Ultimate Mortal Kombat 3",
                    Genero = "Luta",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "Ultimate Mortal Kombat 3 é uma atualização do jogo Mortal Kombat 3 seguido por Mortal Kombat Trilogy. Fora lançada em arcades, SNES, Sega Mega Drive, Sega Saturn, no Xbox Live Arcade do Xbox 360, e na versão de luxo de Mortal Kombat: Armageddon para PS2 e Xbox.",
                    DataLancamento = new DateTime(1990, 11, 21),
                    Emulador = new Emulador
                    {
                        Id = 2,
                        Nome = "Gens",
                        Console = "Mega Drive",
                        Created = DateTime.Now,
                        Novo = true,
                        Descricao = "Emulador de Mega Drive",
                        DataLancamento = new DateTime(1988, 8, 19)
                    }
                });

                return lista;
            }
        }

    }
}
