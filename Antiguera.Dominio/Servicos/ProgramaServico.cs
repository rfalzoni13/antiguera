using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Servicos.Base;

namespace Antiguera.Dominio.Servicos
{
    public class ProgramaServico : ServicoBase<Programa>, IProgramaServico
    {
        private readonly IProgramaRepositorio _programaRepositorio;
        public ProgramaServico(IProgramaRepositorio programaRepositorio)
            :base(programaRepositorio)
        {
            _programaRepositorio = programaRepositorio;
        }

        public void ApagarProgramas(int[] Ids)
        {
            _programaRepositorio.ApagarProgramas(Ids);
        }
    }
}
