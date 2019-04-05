using Antiguera.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Antiguera.WebApi.Teste.ModelsTests
{
    public class Programas
    {
        public List<Programa> ListaProgramas
        {
            get
            {
                var lista = new List<Programa>();

                lista.Add(new Programa()
                {
                    Id = 1,
                    Nome = "Windows 95",
                    Developer = "Microsoft",
                    Publisher = "Microsoft",
                    TipoPrograma = "Sistema Operacional",
                    Created = DateTime.Now,
                    Novo = true,
                    Descricao = "O Microsoft Windows 95 (codinome Chicago) é um sistema operacional de 16/32 bits criado pela empresa Microsoft.",
                    Lancamento = new DateTime(1995, 8, 24)
                });

                lista.Add(new Programa()
                {
                    Id = 2,
                    Nome = "Microsoft Visual Studio 2010",
                    Developer = "Microsoft",
                    Publisher = "Microsoft",
                    TipoPrograma = "IDE Desenvolvimento",
                    Created = DateTime.Now,
                    Novo = false,
                    Descricao = "Microsoft Visual Studio é um ambiente de desenvolvimento integrado (IDE) da Microsoft para desenvolvimento de software especialmente dedicado ao .NET Framework e às linguagens Visual Basic (VB), C, C++, C# (C Sharp) e F# (F Sharp).",
                    Lancamento = new DateTime(2010, 4, 12)
                });

                return lista;
            }
        }

    }
}
