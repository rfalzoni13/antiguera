using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IJogoServico : IServicoBase<Jogo>
    {
        void ApagarJogos(int[] Ids);
    }
}
