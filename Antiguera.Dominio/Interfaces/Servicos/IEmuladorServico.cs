using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IEmuladorServico : IServicoBase<Emulador>
    {
        void ApagarEmuladores(int[] Ids);
    }
}
