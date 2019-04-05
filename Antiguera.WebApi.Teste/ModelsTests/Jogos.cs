using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Jogos
    {
        public List<Jogo> ListaJogos
        {
            get
            {
                var lista = new List<Jogo>();

                lista.Add(new Jogo()
                {
                    Id = 1,
                    Nome = "Doom",
                    Developer = "Id Software",
                    Publisher = "Id Software",
                    Genero = "Tiro em primeira pessoa",
                    Plataforma = "MS-DOS",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "Doom (comercializado como DOOM) é um jogo de computador lançado em 1994 pela id Software e um dos títulos que geraram o gênero tiro em primeira pessoa.",
                    Lancamento = new DateTime(1993, 12, 10)
                });

                lista.Add(new Jogo()
                {
                    Id = 2,
                    Nome = "LHX Attack Chopper",
                    Developer = "Electronic Arts",
                    Publisher = "Electronic Arts",
                    Genero = "Simulador de voô",
                    Plataforma = "MS-DOS",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "LHX Attack Chopper is a 1990 war helicopter simulation game for the PC by Electronic Arts. The game was developed by Electronic Arts, Design and Programming led by Brent Iverson, also known for the PC DOS version of Chuck Yeager's Air Combat, and US Navy Fighters.",
                    Lancamento = new DateTime(1990, 1, 1)
                });

                return lista;
            }
        }
    }
}
