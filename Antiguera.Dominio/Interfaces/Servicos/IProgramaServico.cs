using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IProgramaServico
    {
        ICollection<ProgramaDTO> ListarTodos();
        ProgramaDTO BuscarPorId(Guid id);
        void Adicionar(ProgramaDTO obj);
        void Apagar(ProgramaDTO obj);
        void Atualizar(ProgramaDTO obj);
    }
}
