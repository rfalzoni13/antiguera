using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class JogoServico : ServicoBase<Jogo>, IJogoServico
    {
        private readonly IJogoRepositorio _jogoRepositorio;

        public JogoServico(IJogoRepositorio jogoRepositorio)
            : base(jogoRepositorio)
        {
            _jogoRepositorio = jogoRepositorio;
        }

        public void ApagarJogos(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var jogo = _jogoRepositorio.BuscarPorId(id);

                    if (jogo != null)
                    {
                        _jogoRepositorio.Apagar(jogo);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido!");
            }

        }
    }
}
