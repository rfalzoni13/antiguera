using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IJogoAppServico : IAppServicoBase<Jogo>
    {
        void ApagarJogos(int[] Ids);
    }
}
