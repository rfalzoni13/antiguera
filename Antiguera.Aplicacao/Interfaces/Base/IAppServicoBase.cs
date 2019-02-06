using System;
using System.Collections.Generic;

namespace Antiguera.Aplicacao.Interfaces.Base
{
    public interface IAppServicoBase<T> : IDisposable where T : class
    {
        void Adicionar(T obj);

        void Atualizar(T obj);

        void Apagar(T obj);

        T BuscarPorId(int id);

        IEnumerable<T> BuscarTodos();
    }
}
