using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Servicos.Base;

namespace Antiguera.Dominio.Servicos
{
    public class JogoServico : ServicoBase<Jogo>, IJogoServico
    {
        private readonly IJogoRepositorio _jogoRepositorio;
        public JogoServico(IJogoRepositorio jogoRepositorio)
            :base(jogoRepositorio)
        {
            _jogoRepositorio = jogoRepositorio;
        }

        public void ApagarJogos(int[] Ids)
        {
            _jogoRepositorio.ApagarJogos(Ids);
        }
    }
}
