using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Usuarios
    {
        public List<Usuario> ListaUsuarios
        {
            get
            {
                var lista = new List<Usuario>();

                lista.Add(new Usuario()
                {
                    Id = 1,
                    AcessoId = 1,
                    Nome = "Renato Lopes Falzoni",
                    Email = "renato.lopes.falzoni@gmail.com",
                    Login = "rfalzoni13",
                    Created = DateTime.Now,
                    Sexo = "Masculino",
                    Novo = true,
                    Acesso = new Acesso
                    {
                        Id = 1,
                        Nome = "Administrador",
                        Novo = true,
                        Created = DateTime.Now
                    }
                });

                lista.Add (new Usuario()
                {
                    Id = 2,
                    AcessoId = 2,
                    Nome = "Lilian Lopes da Silva",
                    Email = "lilifofinha@gmail.com",
                    Login = "lilifofinha",
                    Created = DateTime.Now,
                    Sexo = "Feminino",
                    Novo = false,
                    Acesso = new Acesso
                    {
                        Id = 2,
                        Nome = "Usuário",
                        Novo = true,
                        Created = DateTime.Now
                    }
                });

                return lista;
            }
        }
    }
}
