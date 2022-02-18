using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos.Base;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using System;
using System.Collections.Generic;

namespace Antiguera.Servicos.Servicos.Base
{
    public class ServicoBase<TDTO, TEntity> : IServicoBase<TDTO, TEntity> 
        where TDTO : class, new()
        where TEntity : class, new()
    {
        #region Atributos
        private readonly IRepositorioBase<TEntity> _repositorioBase;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IConvertHelper<TDTO, TEntity> _convertToEntity;
        protected private readonly IConvertHelper<TEntity, TDTO> _convertToDTO;
        #endregion

        #region Construtor
        public ServicoBase(IRepositorioBase<TEntity> repositorioBase, IUnitOfWork unitOfWork,
            IConvertHelper<TDTO, TEntity> convertToEntity, IConvertHelper<TEntity, TDTO> convertToDTO)
        {
            _repositorioBase = repositorioBase;
            _unitOfWork = unitOfWork;
            _convertToDTO = convertToDTO;
            _convertToEntity = convertToEntity;
        }
        #endregion

        public virtual void Adicionar(TDTO obj)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (obj != null)
                    {
                        var entity = _convertToEntity.Copy(obj);

                        _repositorioBase.Adicionar(entity);

                        transaction.Commit();
                    }
                    else
                    {
                        throw new ArgumentException("Parâmetro inválido");
                    }
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

        public virtual void Apagar(TDTO obj)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (obj != null)
                    {
                        var entity = _convertToEntity.Copy(obj);

                        _repositorioBase.Apagar(entity);

                        transaction.Commit();
                    }
                    else
                    {
                        throw new ArgumentException("Parâmetro inválido");
                    }
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

        public virtual void Atualizar(TDTO obj)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (obj != null)
                    {
                        var entity = _convertToEntity.Copy(obj);

                        _repositorioBase.Atualizar(entity);

                        transaction.Commit();
                    }
                    else
                    {
                        throw new ArgumentException("Parâmetro inválido");
                    }
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

        public virtual TDTO BuscarPorId(int id)
        {
            var entity = _repositorioBase.BuscarPorId(id);

            var dto = _convertToDTO.Copy(entity);

            return dto;
        }

        public virtual IEnumerable<TDTO> ListarPorPesquisa(Func<TEntity, bool> predicate)
        {
            var listEntities = _repositorioBase.ListarPorPesquisa(predicate);

            var listDTOs = _convertToDTO.CopyList(listEntities);

            return listDTOs;
        }
        
        public virtual IEnumerable<TDTO> ListarTodos()
            => _convertToDTO.CopyList(_repositorioBase.ListarTodos());

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
