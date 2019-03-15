using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;

namespace Antiguera.Servicos
{
    public class UsuarioServico : ServicoBase<Usuario>, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
            : base(usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public void AlterarSenha(int id, string senha)
        {
            _usuarioRepositorio.AlterarSenha(id, senha);
        }

        public void ApagarUsuarios(int[] Ids)
        {
            _usuarioRepositorio.ApagarUsuarios(Ids);
        }

        public void AtualizarNovo(int id)
        {
            _usuarioRepositorio.AtualizarNovo(id);
        }

        public Usuario BuscarUsuarioPorLoginOuEmail(string data) => _usuarioRepositorio.BuscarUsuarioPorLoginOuEmail(data);
    }
}
