using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico : IServicoBase<Usuario>
    {                
        void ApagarUsuarios(int[] Ids);

        void AlterarSenha(int id, string senha);

        Usuario BuscarUsuarioPorLoginOuEmail(string data);
    }
}
