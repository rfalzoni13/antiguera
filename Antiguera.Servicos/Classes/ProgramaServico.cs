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

        public ProgramaServico(IProgramaRepositorio programaRepositorio)
            : base(programaRepositorio)
        {
            _programaRepositorio = programaRepositorio;
        }

        public void ApagarProgramas(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var programa = _programaRepositorio.BuscarPorId(id);

                    if (programa != null)
                    {
                        _programaRepositorio.Apagar(programa);
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
