using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IJogoAppServico : IAppServicoBase<Jogo>
    {
        void AtualizarNovo(int id);

        void ApagarJogos(int[] Ids);
    }
}
