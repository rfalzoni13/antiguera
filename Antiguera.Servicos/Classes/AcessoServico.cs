using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class AcessoServico : ServicoBase<Acesso>, IAcessoServico
    {
        private readonly IAcessoRepositorio _acessoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public AcessoServico(IAcessoRepositorio acessoRepositorio, IUnitOfWork unitOfWork)
            : base(acessoRepositorio, unitOfWork)
        {
            _acessoRepositorio = acessoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void ApagarAcessos(int[] Ids)
        {
            if(Ids != null && Ids.Count() > 0)
            {
                using (_unitOfWork)
                {
                    foreach(var id in Ids)
                    {
                        var acesso = _acessoRepositorio.BuscarPorId(id);

                        if(acesso != null)
                        {
                            _acessoRepositorio.Apagar(acesso);
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
