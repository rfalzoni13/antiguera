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
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IUnitOfWork unitOfWork)
            : base(usuarioRepositorio, unitOfWork)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void AlterarSenha(int id, string senha)
        {
            if(id > 0 && !string.IsNullOrEmpty(senha))
            {
                using (_unitOfWork)
                {
                    var usuario = _usuarioRepositorio.BuscarPorId(id);
                    if(usuario != null)
                    {
                        usuario.Senha = BCrypt.HashPassword(senha, BCrypt.GenerateSalt());
                        _usuarioRepositorio.Atualizar(usuario);
                        _unitOfWork.Commit();
                    }
                }
            }
            else
            {
                throw new ArgumentException("Parâmetros inválidos!");
            }
            
        }

        public void ApagarUsuarios(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                using (_unitOfWork)
                {
                    foreach (var id in Ids)
                    {
                        var usuario = _usuarioRepositorio.BuscarPorId(id);

                        if (usuario != null)
                        {
                            _usuarioRepositorio.Apagar(usuario);
                        }
                    }

                    _unitOfWork.Commit();
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
