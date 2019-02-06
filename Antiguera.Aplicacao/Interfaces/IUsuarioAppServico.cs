using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IUsuarioAppServico : IAppServicoBase<Usuario>
    {
        Usuario FazerLogin(string userName, string password);

        void ApagarUsuarios(int[] Ids);

        void AlterarSenha(int id, string senha);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
