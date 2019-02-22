using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IEmuladorRepositorio : IRepositorioBase<Emulador>
    {
        void AtualizarNovo(int id);

        void ApagarEmuladores(int[] Ids);
    }
}
