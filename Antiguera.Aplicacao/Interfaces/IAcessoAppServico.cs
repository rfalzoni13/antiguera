using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IAcessoAppServico : IAppServicoBase<Acesso>
    {
        void AtualizarNovo(int id);

        void ApagarAcessos(int[] Ids);
    }
}
