using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos.Base
{
    public interface IServicoBase<TDTO, TEntity> : IDisposable 
        where TDTO : class, new()
        where TEntity : class, new()
    {
        void Adicionar(TDTO obj);

        void Atualizar(TDTO obj);

        void Apagar(TDTO obj);

        TDTO BuscarPorId(int id);

        IEnumerable<TDTO> ListarPorPesquisa(Func<TEntity, bool> predicate);

        IEnumerable<TDTO> ListarTodos();
    }
}
