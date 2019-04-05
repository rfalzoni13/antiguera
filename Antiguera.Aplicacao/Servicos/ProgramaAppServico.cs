using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;
using System.Collections.Generic;

namespace Antiguera.Aplicacao.Servicos
{
    public class ProgramaAppServico : AppServicoBase<Programa>, IProgramaAppServico
    {
        private readonly IProgramaServico _programaServico;

        public ProgramaAppServico(IProgramaServico programaServico)
            :base(programaServico)
        {
            _programaServico = programaServico;
        }

        public void ApagarProgramas(int[] Ids)
        {
            _programaServico.ApagarProgramas(Ids);
        }
    }
}
