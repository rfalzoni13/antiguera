using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IRomAppServico : IAppServicoBase<Rom>
    {
        void ApagarRoms(int[] Ids);
    }
}
