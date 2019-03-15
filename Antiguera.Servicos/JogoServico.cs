using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;

namespace Antiguera.Servicos
{
    public class JogoServico : ServicoBase<Jogo>, IJogoServico
    {
        private readonly IJogoRepositorio _jogoRepositorio;
        public JogoServico(IJogoRepositorio jogoRepositorio)
            : base(jogoRepositorio)
        {
            _jogoRepositorio = jogoRepositorio;
        }

        public void ApagarJogos(int[] Ids)
        {
            _jogoRepositorio.ApagarJogos(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _jogoRepositorio.AtualizarNovo(id);
        }
    }
}
