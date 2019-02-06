using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Servicos.Base;

namespace Antiguera.Dominio.Servicos
{
    public class EmuladorServico : ServicoBase<Emulador>, IEmuladorServico
    {
        private readonly IEmuladorRepositorio _emuladorRepositorio;
        public EmuladorServico(IEmuladorRepositorio emuladorRepositorio)
            : base(emuladorRepositorio)
        {
            _emuladorRepositorio = emuladorRepositorio;
        }

        public void ApagarEmuladores(int[] Ids)
        {
            _emuladorRepositorio.ApagarEmuladores(Ids);
        }
    }
}
