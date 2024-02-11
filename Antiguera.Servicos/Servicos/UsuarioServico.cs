using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.Identity;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

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

        public ICollection<UsuarioDTO> ListarTodos()
        {
            var usuarios = UserManager.Users;

            return usuarios.ToList().ConvertAll(u => new UsuarioDTO
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

        public UsuarioDTO ListarPorId(Guid id)
        {
            var usuario = UserManager.FindById(GuidHelper.GuidToString(id));

            return new UsuarioDTO
            {
                Id = GuidHelper.StringToGuid(usuario.Id),
                Nome = $"{usuario.FirstName} {usuario.LastName}",
                Email = usuario.Email,
                Telefone = usuario.PhoneNumber,
                PathFoto = usuario.PhotoPath,
                Genero = usuario.Gender,
                DataNascimento = usuario.DateBirth,
                Login = usuario.UserName,
                Novo = usuario.New,
                Created = usuario.Created,
                Modified = usuario.Modified,
                Acessos = UserManager.GetRoles(usuario.Id).ToArray()
            };
        }
    }
}
