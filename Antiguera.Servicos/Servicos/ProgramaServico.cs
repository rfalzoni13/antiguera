using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Servicos.Servicos.Base;

namespace Antiguera.Servicos.Servicos
{
    public class ProgramaServico : ServicoBase<ProgramaDTO, Programa>, IProgramaServico
    {
        private readonly IProgramaRepositorio _programaRepositorio;

        public ProgramaServico(IProgramaRepositorio programaRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<ProgramaDTO, Programa> convertToEntity, IConvertHelper<Programa, ProgramaDTO> convertToDTO)
            :base(programaRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _programaRepositorio = programaRepositorio;
        }
    }
}
