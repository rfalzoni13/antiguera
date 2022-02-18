using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Servicos.Servicos.Base;

namespace Antiguera.Servicos.Servicos
{
    public class RomServico : ServicoBase<RomDTO, Rom>, IRomServico
    {
        private readonly IRomRepositorio _romRepositorio;

        public RomServico(IRomRepositorio romRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<RomDTO, Rom> convertToEntity, IConvertHelper<Rom, RomDTO> convertToDTO)
            :base(romRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _romRepositorio = romRepositorio;
        }
    }
}
