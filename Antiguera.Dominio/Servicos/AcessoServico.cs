using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Servicos.Base;

namespace Antiguera.Dominio.Servicos
{
    public class AcessoServico : ServicoBase<Acesso>, IAcessoServico
    {
        private readonly IAcessoRepositorio _acessoRepositorio;

        public AcessoServico(IAcessoRepositorio acessoRepositorio)
            :base(acessoRepositorio)
        {
            _acessoRepositorio = acessoRepositorio;
        }

        public void ApagarAcessos(int[] Ids)
        {
            _acessoRepositorio.ApagarAcessos(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _acessoRepositorio.AtualizarNovo(id);
        }
    }
}
