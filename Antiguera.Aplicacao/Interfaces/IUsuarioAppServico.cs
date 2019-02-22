using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IUsuarioAppServico : IAppServicoBase<Usuario>
    {
        void ApagarUsuarios(int[] Ids);

        void AlterarSenha(int id, string senha);

        void AtualizarNovo(int id);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
