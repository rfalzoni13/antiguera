using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;

namespace Antiguera.Aplicacao.Servicos
{
    public class JogoAppServico : AppServicoBase<Jogo>, IJogoAppServico
    {
        private readonly IJogoServico _jogoServico;

        public JogoAppServico(IJogoServico jogoServico)
            :base(jogoServico)
        {
            _jogoServico = jogoServico;
        }

        public void ApagarJogos(int[] Ids)
        {
            _jogoServico.ApagarJogos(Ids);
        }
    }
}
