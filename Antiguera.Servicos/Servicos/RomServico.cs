using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiguera.Servicos.Servicos
{
    public class RomServico : IRomServico
    {
        private readonly IRomRepositorio _romRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public RomServico(IRomRepositorio romRepositorio, IUnitOfWork unitOfWork)
        {
            _romRepositorio = romRepositorio;
            _unitOfWork = unitOfWork;
        }

        public RomDTO BuscarPorId(Guid id)
        {
            var rom = _romRepositorio.BuscarPorId(id);

            return RomDTO.ConvertToDTO(rom);
        }

        public ICollection<RomDTO> ListarTodos()
        {
            var roms = _romRepositorio.ListarTodos();

            return RomDTO.ConvertToList(roms.ToList());
        }

        public void Adicionar(RomDTO obj)
        {
            throw new NotImplementedException();
        }

        public void Apagar(RomDTO obj)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(RomDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
