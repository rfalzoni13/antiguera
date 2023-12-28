using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.Identity;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class AcessoServico : IAcessoServico
    {
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

        public AcessoDTO BuscarPorId(Guid id)
        {
            var acesso = _roleManager.FindById(GuidHelper.GuidToString(id));

            return new AcessoDTO
            {
                Id = GuidHelper.StringToGuid(acesso.Id),
                Novo = acesso.New,
                Created = acesso.Created,
                Modified = acesso.Modified,
            };
        }

        public ICollection<AcessoDTO> ListarTodos()
        {
            var acessos = _roleManager.Roles.ToList();

            return acessos.ToList().ConvertAll(a => new AcessoDTO
            {
                Id = GuidHelper.StringToGuid(a.Id),
                Novo = a.New,
                Created = a.Created,
                Modified = a.Modified,
            });
        }

        public void Adicionar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = new ApplicationRole
                    {
                        Name = obj.Nome
                    };

                    RoleManager.CreateAsync(role);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void Apagar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(GuidHelper.GuidToString(obj.Id)).Result;
                    if (role != null)
                    {
                        RoleManager.DeleteAsync(role);
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

        public void Atualizar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(GuidHelper.GuidToString(obj.Id)).Result;
                    if (role != null)
                    {
                        role.Name = obj.Nome;

                        RoleManager.UpdateAsync(role);
                    }

                    scope.Complete();
                }
                catch(Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }
    }
}
