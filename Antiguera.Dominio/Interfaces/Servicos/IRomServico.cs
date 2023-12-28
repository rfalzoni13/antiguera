using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IRomServico
    {
        ICollection<RomDTO> ListarTodos();
        RomDTO BuscarPorId(Guid id);
        void Adicionar(RomDTO obj);
        void Apagar(RomDTO obj);
        void Atualizar(RomDTO obj);

    }
}
