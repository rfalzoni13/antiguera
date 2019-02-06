using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        Usuario FazerLogin(string userName, string password);

        void ApagarUsuarios(int[] Ids);

        void AlterarSenha(int id, string senha);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
