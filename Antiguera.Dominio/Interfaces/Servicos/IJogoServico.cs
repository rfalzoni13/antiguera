using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IJogoServico
    {
        ICollection<JogoDTO> ListarTodos();
        JogoDTO BuscarPorId(Guid id);
        void Adicionar(JogoDTO obj);
        void Apagar(JogoDTO obj);
        void Atualizar(JogoDTO obj);

    }
}
