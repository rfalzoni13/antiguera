using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients.Base
{
    public interface IClientBase<T, TTable> 
        where T : class
        where TTable : class
    {
        Task<T> Listar(string url, string id);
        Task<ICollection<T>> ListarTodos(string url);
        Task<TTable> ListarTabela(string url);
        Task<string> Inserir(string url, T obj);
        Task<string> Atualizar(string url, T obj);
        Task<string> Excluir(string url, T obj);
    }
}
