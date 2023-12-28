using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiguera.Servicos.Servicos
{
    public class ProgramaServico : IProgramaServico
    {
        private readonly IProgramaRepositorio _programaRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramaServico(IProgramaRepositorio programaRepositorio, IUnitOfWork unitOfWork)
        {
            _programaRepositorio = programaRepositorio;
            _unitOfWork = unitOfWork;
        }

        public ProgramaDTO BuscarPorId(Guid id)
        {
            var programa = _programaRepositorio.BuscarPorId(id);

            return ProgramaDTO.ConvertToDTO(programa);
        }

        public ICollection<ProgramaDTO> ListarTodos()
        {
            var programas = _programaRepositorio.ListarTodos();

            return ProgramaDTO.ConvertToList(programas.ToList());
        }

        public void Adicionar(ProgramaDTO obj)
        {
            throw new NotImplementedException();
        }

        public void Apagar(ProgramaDTO obj)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(ProgramaDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
