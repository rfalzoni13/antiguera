using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class ProgramaServico : ServicoBase<Programa>, IProgramaServico
    {
        private readonly IProgramaRepositorio _programaRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramaServico(IProgramaRepositorio programaRepositorio, IUnitOfWork unitOfWork)
            : base(programaRepositorio, unitOfWork)
        {
            _programaRepositorio = programaRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void ApagarProgramas(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                using (_unitOfWork)
                {
                    foreach (var id in Ids)
                    {
                        var programa = _programaRepositorio.BuscarPorId(id);

                        if (programa != null)
                        {
                            _programaRepositorio.Apagar(programa);
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
