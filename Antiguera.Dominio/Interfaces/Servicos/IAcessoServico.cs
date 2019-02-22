using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IAcessoServico : IServicoBase<Acesso>
    {
        void AtualizarNovo(int id);

        void ApagarAcessos(int[] Ids);
    }
}
