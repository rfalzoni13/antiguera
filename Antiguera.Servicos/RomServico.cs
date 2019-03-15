using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;

namespace Antiguera.Servicos
{
    public class RomServico : ServicoBase<Rom>, IRomServico
    {
        private readonly IRomRepositorio _romRepositorio;
        public RomServico(IRomRepositorio romRepositorio)
            : base(romRepositorio)
        {
            _romRepositorio = romRepositorio;
        }

        public void ApagarRoms(int[] Ids)
        {
            _romRepositorio.ApagarRoms(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _romRepositorio.AtualizarNovo(id);
        }
    }
}
