using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;

namespace Antiguera.Aplicacao.Servicos
{
    public class AcessoAppServico : AppServicoBase<Acesso>, IAcessoAppServico
    {
        private readonly IAcessoServico _acessoServico;

        public AcessoAppServico(IAcessoServico acessoServico)
            :base(acessoServico)
        {
            _acessoServico = acessoServico;
        }

        public void ApagarAcessos(int[] Ids)
        {
            _acessoServico.ApagarAcessos(Ids);
        }
    }
}
