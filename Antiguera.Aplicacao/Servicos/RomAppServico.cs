using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;

namespace Antiguera.Aplicacao.Servicos
{
    public class RomAppServico : AppServicoBase<Rom>, IRomAppServico
    {
        private readonly IRomServico _romServico;

        public RomAppServico(IRomServico romServico)
            : base(romServico)
        {
            _romServico = romServico;
        }

        public void ApagarRoms(int[] Ids)
        {
            _romServico.ApagarRoms(Ids);
        }
    }
}
