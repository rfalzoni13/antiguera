using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Emuladores
    {
        public List<Emulador> ListaEmuladores
        {
            get
            {
                var lista = new List<Emulador>();

                lista.Add(new Emulador()
                {
                    Id = 1,
                    Nome = "ZSnes",
                    Console = "Super Nintendo",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "Emulador de Super Nintendo",
                    DataLancamento = new DateTime(1997, 1, 1)
                });

                lista.Add(new Emulador()
                {
                    Id = 2,
                    Nome = "Gens",
                    Console = "Mega Drive",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "Emulador de Mega Drive",
                    DataLancamento = new DateTime(1988, 8, 19)
                });

                return lista;
            }
        }
    }
}
