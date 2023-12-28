using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        ICollection<UsuarioDTO> ListarTodos();
        UsuarioDTO ListarPorId(Guid id);
    }
}
