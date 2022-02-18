using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Antiguera.Administrador.Helpers
{
    public static class AcessoHelper
    {
        private static ApplicationSignInManager signInManager;
        private static ApplicationUserManager userManager;
        private static ApplicationRoleManager roleManager;

        public static List<string> ErrorsList { get; set; }

        #region Asp Net Identity Managers
        private static ApplicationSignInManager SignInManager()
        {
            return signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        private static ApplicationUserManager UserManager()
        {
            return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        private static ApplicationRoleManager RoleManager()
        {
            return roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
        }
        #endregion

        protected static async Task CadastrarAcesso(IAcessoServico acessoServico, Acesso acesso)
        {
            List<string> errorsList = new List<string>();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    if (acesso != null)
                    {
                        var role = await RoleManager().FindByNameAsync(acesso.Nome);
                        if (role == null)
                        {
                            role = new IdentityRole(acesso.Nome);
                            var result = await RoleManager().CreateAsync(role);

                            if (result.Succeeded)
                            {
                                acesso.IdentityRoleId = role.Id;
                            }
                            else
                            {
                                errorsList.Add("Erro ao inserir acesso!");
                            }
                        }

                        if (errorsList.Count() <= 0)
                        {
                            acesso.Created = DateTime.Now;

                            acesso.Novo = true;

                            acessoServico.Adicionar(acesso);

                            _logger.Info("Acesso incluído com sucesso!");

                            _logger.Info("Finalizado");

                            var idUsuario = Convert.ToInt32(SignInManager().AuthenticationManager.User.Claims
                                    .Where(x => x.Type.Equals("UsuarioId")).Select(x => x.Value).FirstOrDefault());

                            GravarHistorico(idUsuario, ETipoHistorico.InserirAcesso);
                        }
                    }
                    else
                    {
                        errorsList.Add("Parâmetros incorretos");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal("Ocorreu um erro! " + ex);
                    errorsList.Add("Ocorreu um erro grave, consulte arquivo de logs para mais informações!");
                }
            }
        }
    }
}