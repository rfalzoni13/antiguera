using Antiguera.Administrador.Client.Base;
using Antiguera.Administrador.Models;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Client.Interface
{
    public interface IUsuarioClient : IClientBase<UsuarioModel>
    {
        Task<UsuarioModel> ListarPorIdentityId(string userId, string token);
    }
}
