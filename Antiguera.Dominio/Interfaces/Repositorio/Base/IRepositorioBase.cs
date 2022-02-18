using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antiguera.Dominio.Interfaces.Repositorio.Base
{
    public interface IRepositorioBase<T> where T : class
    {
        void Adicionar(T obj);

        void Atualizar(T obj);

        void Apagar(T obj);

        T BuscarPorId(int id);

        IEnumerable<T> ListarTodos();

        IEnumerable<T> ListarPorPesquisa(Func<T, bool> predicate);
    }
}
