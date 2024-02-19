using Antiguera.Dominio.DTO;
using Antiguera.Servicos.IdentityConfiguration;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Infra.Data.Identity;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using System.Threading.Tasks;
using System.IO;

namespace Antiguera.Servicos.Servicos.Identity
{
    public class UsuarioServico : IDisposable
    {
        #region Atributos
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

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

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
        #endregion

        #region Pesquisas
        public ICollection<UsuarioDTO> ListarTodosUsuarios()
        {
            var users = UserManager.Users;

            return users.ToList().ConvertAll(u => new UsuarioDTO
            {
                Id = GuidHelper.StringToGuid(u.Id),
                Nome = $"{u.FirstName} {u.LastName}",
                Email = u.Email,
                Telefone = u.PhoneNumber,
                PathFoto = u.PhotoPath,
                Genero = u.Gender,
                DataNascimento = u.DateBirth,
                Login = u.UserName,
                Novo = u.New,
                Created = u.Created,
                Modified = u.Modified,
                Acessos = UserManager.GetRoles(u.Id).ToArray()
            });
        }

        public UsuarioDTO ListarUsuarioPorId(Guid Id)
        {
            var user = UserManager.FindById(GuidHelper.GuidToString(Id));

            return new UsuarioDTO
            {
                Id = GuidHelper.StringToGuid(user.Id),
                Nome = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Telefone = user.PhoneNumber,
                PathFoto = user.PhotoPath,
                Genero = user.Gender,
                DataNascimento = user.DateBirth,
                Login = user.UserName,
                Novo = user.New,
                Created = user.Created,
                Modified = user.Modified,
                Acessos = UserManager.GetRoles(user.Id).ToArray()
            };
        }
        #endregion

        #region Services
        public void Adicionar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Adicionar usuário
                    var user = UserManager.FindByNameAsync(register.Login).Result;

                    if (user != null)
                    {
                        throw new ApplicationException("Já existe um usuário com estas informações!");
                    }

                    user = new ApplicationUser()
                    {
                        FirstName = register.Nome.Split(' ').FirstOrDefault(),
                        LastName = register.Nome.Split(' ').LastOrDefault(),
                        Email = register.Email,
                        PhoneNumber = register.Telefone,
                        DateBirth = register.DataNascimento,
                        Gender = register.Genero,
                        UserName = register.Login,
                        Active = true,
                        Created = DateTime.Now
                    };

                    var result = UserManager.Create(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao incluir usuário!");
                    }

                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                        FileHelper.IncludeFileFromBase64("/Attachments/Usuario", user.Id, user.PhotoPath);

                        result = UserManager.Update(user);
                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao atualizar foto do usuário");
                        }
                    }

                    //Adicionar acesso
                    int i = 0;

                    while (i < register.Acessos.Count())
                    {
                        var role = RoleManager.FindByName(register.Acessos[i]);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        i++;

                        result = UserManager.AddToRole(user.Id, role.Name);

                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao incluir perfil de acesso!");
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void Apagar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = UserManager.FindById(register.ID);

                    if (user == null)
                    {
                        throw new ArgumentNullException("Nenhum usuário encontrado!");
                    }

                    var result = UserManager.Delete(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao excluir usuário!");
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void Atualizar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Atualizar usuário
                    var user = UserManager.FindById(register.ID);

                    if (user == null)
                    {
                        throw new ApplicationException("Nenhum usuário encontrado!");
                    }

                    user.FirstName = register.Nome.Split(' ').FirstOrDefault();
                    user.LastName = register.Nome.Split(' ').LastOrDefault();
                    user.Email = register.Email;
                    user.DateBirth = register.DataNascimento;
                    user.PhotoPath = register.PathFoto;
                    user.PhoneNumber = register.Telefone;
                    user.Modified = DateTime.Now;

                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                        string path = Path.Combine("/Attachments/Usuario", user.Id);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        FileHelper.IncludeFileFromBase64("/Attachments/Usuario", user.Id, user.PhotoPath);
                    }

                    var result = UserManager.Update(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar usuário!");
                    }

                    //Atualizar acesso
                    int i = 0;

                    do
                    {
                        var role = RoleManager.FindById(register.ID);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        var roles = UserManager.GetRolesAsync(user.Id).Result;
                        if (roles == null || roles.Count() <= 0)
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
                    }
                    while (i < register.Acessos.Count());

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }
        #endregion

        #region Async Services
        public async Task AdicionarAsnyc(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Adicionar usuário
                    var user = UserManager.FindByNameAsync(register.Login).Result;

                    if (user != null)
                    {
                        throw new ApplicationException("Já existe um usuário com estas informações!");
                    }

                    user = new ApplicationUser()
                    {
                        FirstName = register.Nome.Split(' ').FirstOrDefault(),
                        LastName = register.Nome.Split(' ').LastOrDefault(),
                        Email = register.Email,
                        PhoneNumber = register.Telefone,
                        DateBirth = register.DataNascimento,
                        Gender = register.Genero,
                        UserName = register.Login,
                        Active = true,
                        Created = DateTime.Now
                    };

                    var result = await UserManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao incluir usuário!");
                    }

                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                        FileHelper.IncludeFileFromBase64("/Attachments/Usuario", user.Id, user.PhotoPath);

                        result = await UserManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao atualizar foto do usuário");
                        }
                    }

                    //Adicionar acesso
                    int i = 0;

                    while (i < register.Acessos.Count())
                    {
                        var role = await RoleManager.FindByNameAsync(register.Acessos[i]);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        i++;

                        result = await UserManager.AddToRoleAsync(user.Id, role.Name);

                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao incluir perfil de acesso!");
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public async Task ApagarAsync(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(register.ID);

                    if (user == null)
                    {
                        throw new ArgumentNullException("Nenhum usuário encontrado!");
                    }

                    var result = await UserManager.DeleteAsync(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao excluir usuário!");
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public async Task AtualizarAsync(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Atualizar usuário
                    var user = await UserManager.FindByIdAsync(register.ID);

                    if (user == null)
                    {
                        throw new ApplicationException("Nenhum usuário encontrado!");
                    }

                    user.FirstName = register.Nome.Split(' ').FirstOrDefault();
                    user.LastName = register.Nome.Split(' ').LastOrDefault();
                    user.Email = register.Email;
                    user.DateBirth = register.DataNascimento;
                    user.PhotoPath = register.PathFoto;
                    user.PhoneNumber = register.Telefone;
                    user.Modified = DateTime.Now;

                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                        string path = Path.Combine("/Attachments/Usuario", user.Id);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        FileHelper.IncludeFileFromBase64("/Attachments/Usuario", user.Id, user.PhotoPath);
                    }

                    var result = await UserManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar usuário!");
                    }

                    //Atualizar acesso
                    int i = 0;

                    do
                    {
                        var role = await RoleManager.FindByNameAsync(register.ID);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        var roles = UserManager.GetRolesAsync(user.Id).Result;
                        if (roles == null || roles.Count() <= 0)
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
                    }
                    while (i < register.Acessos.Count());

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            RoleManager.Dispose();
            SignInManager.Dispose();
            UserManager.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
