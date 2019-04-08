using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class UsuarioServico : ServicoBase<Usuario>, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
            : base(usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public void ApagarUsuarios(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var usuario = _usuarioRepositorio.BuscarPorId(id);

                    if (usuario != null)
                    {
                        _usuarioRepositorio.Apagar(usuario);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido!");
            }

        }

        public Usuario BuscarUsuarioPorLoginOuEmail(string data) 
            => !string.IsNullOrEmpty(data) ? _usuarioRepositorio.BuscarUsuarioPorLoginOuEmail(data)
            : throw new ArgumentException("Parâmetro inválido!");
    }
}
