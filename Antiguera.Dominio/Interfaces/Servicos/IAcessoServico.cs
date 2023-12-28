using Antiguera.Dominio.DTO;
using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IAcessoServico
    {
        ICollection<AcessoDTO> ListarTodos();
        AcessoDTO BuscarPorId(Guid id);
        void Adicionar(AcessoDTO obj);
        void Apagar(AcessoDTO obj);
        void Atualizar(AcessoDTO obj);
    }
}
