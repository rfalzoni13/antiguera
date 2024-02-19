using Antiguera.Dominio.DTO;
using Antiguera.Servicos.IdentityConfiguration;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Antiguera.Servicos.Servicos.Identity
{
    public class AcessoServico
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

        #region Acesso
        public List<string> ListarTodosNomesAcessos()
        {
            var roles = RoleManager.Roles;

            return roles.Select(x => x.Name).Distinct().ToList();
        }
        
        public ICollection<AcessoDTO> ListarTodosAcessos()
        {
            var roles = RoleManager.Roles;

            return roles.ToList().ConvertAll(r => new AcessoDTO
            {
                Id = GuidHelper.StringToGuid(r.Id),
                Nome = r.Name,
                Novo = r.New,
                Created = r.Created,
                Modified = r.Modified,
            });
        }

        public AcessoDTO ListarAcessoPorId(Guid Id)
        {
            var role = RoleManager.FindById(GuidHelper.GuidToString(Id));

            return new AcessoDTO
            {
                Id = GuidHelper.StringToGuid(role.Id),
                Nome = role.Name,
                Novo = role.New,
                Created = role.Created,
                Modified = role.Modified,
            };
        }
        #endregion
    }
}
