using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;

namespace Antiguera.Servicos
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
            _programaRepositorio.ApagarProgramas(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _programaRepositorio.AtualizarNovo(id);
        }
    }
}
