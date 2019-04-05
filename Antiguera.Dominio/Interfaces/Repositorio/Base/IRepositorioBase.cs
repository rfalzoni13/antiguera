using System;
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Repositorio.Base
{
    public interface IRepositorioBase<T> where T : class
    {
        void Adicionar(T obj);

        void Atualizar(T obj);

        void Apagar(T obj);

        T BuscarPorId(int id);

        IEnumerable<T> BuscarTodos();

        IEnumerable<T> BuscaQuery(Func<T, bool> predicate);
    }
}
