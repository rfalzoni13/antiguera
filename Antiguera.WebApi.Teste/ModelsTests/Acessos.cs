using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Acessos
    {
        public List<Acesso> ListaAcessos
        {
            get
            {
                var lista = new List<Acesso>();
                lista.Add(new Acesso()
                {
                    Id = 1,
                    Nome = "Administrador",
                    Novo = true,
                    Created = DateTime.Now
                });

                lista.Add(new Acesso()
                {
                    Id = 2,
                    Nome = "Usuário",
                    Novo = true,
                    Created = DateTime.Now
                });

                return lista;
            }
        }
    }
}
