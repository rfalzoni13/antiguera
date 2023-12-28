using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiguera.Servicos.Servicos
{
    public class EmuladorServico : IEmuladorServico
    {
        private readonly IEmuladorRepositorio _emuladorRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public EmuladorServico(IEmuladorRepositorio emuladorRepositorio, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _emuladorRepositorio = emuladorRepositorio;
        }

        public EmuladorDTO BuscarPorId(Guid id)
        {
            var emulador = _emuladorRepositorio.BuscarPorId(id);

            return EmuladorDTO.ConvertToDTO(emulador);
        }

        public ICollection<EmuladorDTO> ListarTodos()
        {
            var emuladores = _emuladorRepositorio.ListarTodos();

            return EmuladorDTO.ConvertToList(emuladores.ToList());
        }


        public void Adicionar(EmuladorDTO obj)
        {
            throw new System.NotImplementedException();
        }

        public void Apagar(EmuladorDTO obj)
        {
            throw new System.NotImplementedException();
        }

        public void Atualizar(EmuladorDTO obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
