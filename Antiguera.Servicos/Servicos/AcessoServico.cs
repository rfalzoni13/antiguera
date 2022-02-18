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
using System.Transactions;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class AcessoServico : ServicoBase<AcessoDTO, Acesso>, IAcessoServico
    {
        private ApplicationRoleManager _roleManager;
        private readonly IAcessoRepositorio _acessoRepositorio;
        private readonly IHistoricoRepositorio _historicoRepositorio;

        public AcessoServico(IAcessoRepositorio acessoRepositorio, IHistoricoRepositorio historicoRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<AcessoDTO, Acesso> convertToEntity, IConvertHelper<Acesso, AcessoDTO> convertToDTO)
            :base(acessoRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _acessoRepositorio = acessoRepositorio;
            _historicoRepositorio = historicoRepositorio;
        }

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

        public override void Adicionar(AcessoDTO obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var role = new ApplicationRole
                    {
                        Name = obj.Nome
                    };

                    RoleManager.CreateAsync(role);

                    obj.IdentityRoleId = role.Id;

                    base.Adicionar(obj);

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

        public override void Apagar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(obj.IdentityRoleId).Result;
                    if (role != null)
                    {
                        RoleManager.DeleteAsync(role);
                    }

                    base.Apagar(obj);

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

        public override void Atualizar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(obj.IdentityRoleId).Result;
                    if (role != null)
                    {
                        role.Name = obj.Nome;

                        RoleManager.UpdateAsync(role);
                    }

                    base.Atualizar(obj);

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
    }
}
