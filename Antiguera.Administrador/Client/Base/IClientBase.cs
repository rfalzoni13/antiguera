using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Client.Base
{
    public interface IClientBase<T> where T : class
    {
        Task<T> Listar(string url);
        Task<ICollection<T>> ListarTodos(string url);
        Task<string> Inserir(string url, T obj);
        Task<string> Atualizar(string url, T obj);
        Task<string> Excluir(string url, T obj);
    }
}
