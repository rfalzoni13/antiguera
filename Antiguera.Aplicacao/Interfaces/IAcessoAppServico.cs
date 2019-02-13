using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IAcessoAppServico : IAppServicoBase<Acesso>
    {
        void ApagarAcessos(int[] Ids);
    }
}
