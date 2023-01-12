using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.Identity;
using Antiguera.Servicos.Servicos.Base;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
                        PhoneNumber = usuarioDTO.Telefone,
                        UserName = usuarioDTO.Login,
                        Active = true,
                        Created = DateTime.Now
                    };

                    var result = UserManager.CreateAsync(user, usuarioDTO.Senha).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao incluir usuário!");
                    }


                    result = UserManager.AddToRoleAsync(user.Id, role.Name).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao incluir perfil de acesso!");
                    }

                    var usuario = new Usuario
                    {
                        Novo = true,
                        IdentityUserId = user.Id,
                        Created = DateTime.Now,
                        PathFoto = usuarioDTO.PathFoto
                    };

                    _usuarioRepositorio.Adicionar(usuario);

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

        public override void Atualizar(UsuarioDTO usuarioDTO)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var role = RoleManager.FindByNameAsync(usuarioDTO.Acesso.Nome).Result;
                    if (role == null)
                    {
                        throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                    }

                    var user = UserManager.FindByIdAsync(usuarioDTO.IdentityUserId).Result;

                    if (user == null)
                    {
                        throw new ApplicationException("Nenhum usuário encontrado!");
                    }

                    user.FirstName = usuarioDTO.Nome.Split(' ').FirstOrDefault();
                    user.LastName = usuarioDTO.Nome.Split(' ').LastOrDefault();
                    user.Email = usuarioDTO.Email;
                    user.PhoneNumber = usuarioDTO.Telefone;
                    user.Modified = DateTime.Now;

                    var result = UserManager.UpdateAsync(user).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar usuário!");
                    }

                    var roles = UserManager.GetRolesAsync(user.Id).Result;
                    if(roles == null || roles.Count() <= 0)
                    {
                        throw new ApplicationException("Erro ao cadastrar novo perfil de acesso!");
                    }

                    result = UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray()).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao realizar manutenção de acesso!");
                    }

                    result = UserManager.AddToRoleAsync(user.Id, role.Name).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar acesso!");
                    }

                    var usuario = _usuarioRepositorio.BuscarPorIdentityId(usuarioDTO.IdentityUserId);
                    if (usuario == null)
                    {
                        throw new ApplicationException("Usuário não encontrado!");
                    }

                    usuario.PathFoto = usuarioDTO.PathFoto;

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

        public UsuarioDTO ListarPorIdentityId(string userId)
        {
            var usuario = _usuarioRepositorio.BuscarPorIdentityId(userId);

            var user = UserManager.FindByIdAsync(userId).Result;

            if (usuario == null || user == null)
                throw new ApplicationException("Usuário não encontrado!");

            return new UsuarioDTO
            {
                Id = usuario.Id,
                IdentityUserId = usuario.IdentityUserId,
                AcessoId = usuario.AcessoId,
                DataNascimento = usuario.DataNascimento,
                Nome = usuario.Nome,
                Email = user?.Email,
                Genero = usuario.Genero,
                Login = user?.UserName,
                Novo = usuario.Novo,
                PathFoto = usuario.PathFoto,
                Telefone = usuario.Telefone,
                UltimaVisita = DateTime.Now,
                Created = usuario.Created,
                Modified = usuario.Modified,
                Acesso = new AcessoDTO
                {
                    Id = usuario.Acesso.Id,
                    Nome = usuario.Acesso.Nome,
                    IdentityRoleId = usuario.Acesso.IdentityRoleId,
                    Created = usuario.Acesso.Created,
                    Modified = usuario.Acesso.Modified,
                    Novo = usuario.Acesso.Novo
                }
            };
        }
    }
}
