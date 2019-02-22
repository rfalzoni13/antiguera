using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;

namespace Antiguera.Aplicacao.Servicos
{
    public class EmuladorAppServico : AppServicoBase<Emulador>, IEmuladorAppServico
    {
        private readonly IEmuladorServico _emuladorServico;

        public EmuladorAppServico(IEmuladorServico emuladorServico)
            : base(emuladorServico)
        {
            _emuladorServico = emuladorServico;
        }

        public void ApagarEmuladores(int[] Ids)
        {
            _emuladorServico.ApagarEmuladores(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _emuladorServico.AtualizarNovo(id);
        }
    }
}
