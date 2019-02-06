using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Repositorio.Base
{
    public interface IRepositorioBase<T> where T : class
    {
        void Adicionar(T obj);

        void Atualizar(T obj);

        T BuscarPorId(int id);

        IEnumerable<T> BuscarTodos();

        void Apagar(T obj);
    }
}
