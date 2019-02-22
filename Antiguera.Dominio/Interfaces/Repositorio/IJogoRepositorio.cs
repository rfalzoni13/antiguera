using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IJogoRepositorio : IRepositorioBase<Jogo>
    {
        void AtualizarNovo(int id);

        void ApagarJogos(int[] Ids);
    }
}
