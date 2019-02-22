using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;
using System.Collections.Generic;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IProgramaAppServico : IAppServicoBase<Programa>
    {
        void AtualizarNovo(int id);

        void ApagarProgramas(int[] Ids);
    }
}
