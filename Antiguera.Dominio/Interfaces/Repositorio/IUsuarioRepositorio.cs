using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        void ApagarUsuarios(int[] Ids);

        void AlterarSenha(int id, string senha);

        void AtualizarNovo(int id);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
