using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients.Interface
{
    public interface IUsuarioClient : IClientBase<UsuarioModel, UsuarioTableModel>
    {
        Task<UsuarioModel> ListarPorIdentityId(string userId, string token);
    }
}
