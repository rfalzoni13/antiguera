using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Entidades;

namespace Antiguera.Aplicacao.Interfaces
{
    public interface IUsuarioAppServico : IAppServicoBase<Usuario>
    {
        void ApagarUsuarios(int[] Ids);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
