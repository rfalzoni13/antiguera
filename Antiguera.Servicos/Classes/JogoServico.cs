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
        private readonly IUnitOfWork _unitOfWork;

        public JogoServico(IJogoRepositorio jogoRepositorio, IUnitOfWork unitOfWork)
            : base(jogoRepositorio, unitOfWork)
        {
            _jogoRepositorio = jogoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void ApagarJogos(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                using (_unitOfWork)
                {
                    foreach (var id in Ids)
                    {
                        var jogo = _jogoRepositorio.BuscarPorId(id);

                        if (jogo != null)
                        {
                            _jogoRepositorio.Apagar(jogo);
                        }
                    }

                    _unitOfWork.Commit();
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido!");
            }

        }
    }
}
