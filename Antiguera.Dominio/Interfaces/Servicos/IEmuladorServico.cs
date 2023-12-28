using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IEmuladorServico
    {
        ICollection<EmuladorDTO> ListarTodos();
        EmuladorDTO BuscarPorId(Guid id);
        void Adicionar(EmuladorDTO obj);
        void Apagar(EmuladorDTO obj);
        void Atualizar(EmuladorDTO obj);
    }
}
