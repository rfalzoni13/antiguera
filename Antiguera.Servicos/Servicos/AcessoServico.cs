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

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = new ApplicationRole
                    {
                        Name = obj.Nome
                    };

                    RoleManager.CreateAsync(role);

                    obj.IdentityRoleId = role.Id;

                    using(var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _acessoRepositorio.Adicionar(Acesso.ConvertToEntity(obj));

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

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public override void Apagar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(obj.IdentityRoleId).Result;
                    if (role != null)
                    {
                        RoleManager.DeleteAsync(role);
                    }

                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _acessoRepositorio.Apagar(Acesso.ConvertToEntity(obj));
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

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public override void Atualizar(AcessoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Nenhum objeto encontrado!");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = RoleManager.FindByIdAsync(obj.IdentityRoleId).Result;
                    if (role != null)
                    {
                        role.Name = obj.Nome;

                        RoleManager.UpdateAsync(role);
                    }

                    using(var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _acessoRepositorio.Atualizar(Acesso.ConvertToEntity(obj));
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
