using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Client.Base
{
    public interface IClientBase<T> where T : class
    {
        Task<T> Listar(string url, string token);
        Task<ICollection<T>> ListarTodos(string url, string token);
        Task<string> Inserir(string url, string token, T obj);
        Task<string> Atualizar(string url, string token, T obj);
        Task<string> Excluir(string url, string token, T obj);
    }
}
