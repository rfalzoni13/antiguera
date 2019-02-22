using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IEmuladorAppServico : IAppServicoBase<Emulador>
    {
        void AtualizarNovo(int id);

        void ApagarEmuladores(int[] Ids);
    }
}
