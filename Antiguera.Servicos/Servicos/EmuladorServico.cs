using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Servicos.Servicos.Base;

namespace Antiguera.Servicos.Servicos
{
    public class EmuladorServico : ServicoBase<EmuladorDTO, Emulador>, IEmuladorServico
    {
        private readonly IEmuladorRepositorio _emuladorRepositorio;
        public EmuladorServico(IEmuladorRepositorio emuladorRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<EmuladorDTO, Emulador> convertToEntity, IConvertHelper<Emulador, EmuladorDTO> convertToDTO)
            : base(emuladorRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _emuladorRepositorio = emuladorRepositorio;
        }
    }
}
