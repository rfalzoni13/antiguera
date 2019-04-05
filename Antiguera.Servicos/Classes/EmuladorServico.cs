using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class EmuladorServico : ServicoBase<Emulador>, IEmuladorServico
    {
        private readonly IEmuladorRepositorio _emuladorRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        public EmuladorServico(IEmuladorRepositorio emuladorRepositorio, IUnitOfWork unitOfWork)
            : base(emuladorRepositorio, unitOfWork)
        {
            _emuladorRepositorio = emuladorRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void ApagarEmuladores(int[] Ids)
        {
            if(Ids != null && Ids.Count() > 0)
            {
                using (_unitOfWork)
                {
                    foreach (var id in Ids)
                    {
                        var emulador = _emuladorRepositorio.BuscarPorId(id);
                        if (emulador != null)
                        {
                            _emuladorRepositorio.Apagar(emulador);
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
