using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Infra.Cross.Identity;
using Antiguera.Servicos.Servicos.Base;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class UsuarioServico : ServicoBase<UsuarioDTO, Usuario>, IUsuarioServico
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }



        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<UsuarioDTO, Usuario> convertToEntity, IConvertHelper<Usuario, UsuarioDTO> convertToDTO)
            : base(usuarioRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public override void Adicionar(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Novo = true,
                Created = DateTime.Now,
                pathFoto = usuarioDTO.pathFoto
            };

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var role = RoleManager.FindByNameAsync(usuarioDTO.Acesso.Nome).Result;
                    if (role == null)
                    {
                        throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                    }

                    var user = UserManager.FindByNameAsync(usuarioDTO.Login).Result;

                    if (user != null)
                    {
                        throw new ApplicationException("Já existe um usuário com estas informações!");
                    }

                    user = new ApplicationUser()
                    {
                        FirstName = usuarioDTO.Nome.Split(' ').FirstOrDefault(),
                        LastName = usuarioDTO.Nome.Split(' ').LastOrDefault(),
                        Email = usuarioDTO.Email,
                        UserName = usuarioDTO.Login,
                        Active = true,
                        Created = DateTime.Now
                    };

                    var result = UserManager.CreateAsync(user, usuarioDTO.Senha).Result;

                    if (result.Succeeded)
                    {
                        usuario.IdentityUserId = user.Id;
                    }
                    else
                    {
                        throw new DbUpdateException("Erro ao incluir usuário!");
                    }

                    UserManager.AddToRoleAsync(user.Id, role.Name);

                    _usuarioRepositorio.Adicionar(usuario);

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public override void Apagar(UsuarioDTO obj)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {

                    var user = UserManager.FindByIdAsync(obj.IdentityUserId).Result;

                    if (user == null)
                    {
                        throw new ArgumentNullException("Nenhum usuário encontrado!");
                    }

                    var result = UserManager.DeleteAsync(user).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao excluir usuário!");
                    }

                    var usuario = _usuarioRepositorio.BuscarPorId(obj.Id);

                    _usuarioRepositorio.Apagar(usuario);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public override void Atualizar(UsuarioDTO usuarioDTO)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var usuario = _usuarioRepositorio.BuscarPorId(usuarioDTO.Id);

                try
                {
                    var role = RoleManager.FindByNameAsync(usuarioDTO.Acesso.Nome).Result;
                    if (role == null)
                    {
                        throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                    }

                    var user = UserManager.FindByIdAsync(usuario.IdentityUserId).Result;

                    if (user == null)
                    {
                        throw new ApplicationException("Nenhum usuário encontrado!");
                    }

                    user.FirstName = usuarioDTO.Nome.Split(' ').FirstOrDefault();
                    user.LastName = usuarioDTO.Nome.Split(' ').LastOrDefault();
                    user.Email = usuarioDTO.Email;
                    user.Modified = DateTime.Now;

                    var result = UserManager.UpdateAsync(user).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar usuário!");
                    }

                    var roles = UserManager.GetRolesAsync(user.Id).Result;
                    UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                    UserManager.AddToRoleAsync(user.Id, role.Name);

                    _usuarioRepositorio.Atualizar(usuario);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
    }
}
