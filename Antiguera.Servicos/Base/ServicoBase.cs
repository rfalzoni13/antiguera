using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Servicos.Base
{
    public class ServicoBase<T> : IServicoBase<T> where T : class
    {
        #region Atributos
        private readonly IRepositorioBase<T> _repositorioBase;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Construtor
        public ServicoBase(IRepositorioBase<T> repositorioBase, IUnitOfWork unitOfWork)
        {
            _repositorioBase = repositorioBase;
            _unitOfWork = unitOfWork;
        }
        #endregion

        public void Adicionar(T obj)
        {
            if (obj != null)
            {
                using (_unitOfWork)
                {
                    _repositorioBase.Adicionar(obj);

                    _unitOfWork.Commit();
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido");
            }
        }

        public void Apagar(T obj)
        {
            if (obj != null)
            {
                using (_unitOfWork)
                {
                    _repositorioBase.Apagar(obj);

                    _unitOfWork.Commit();
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido");
            }
        }

        public void Atualizar(T obj)
        {
            if (obj != null)
            {
                using (_unitOfWork)
                {
                    _repositorioBase.Atualizar(obj);

                    _unitOfWork.Commit();
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido");
            }
        }

        public IEnumerable<T> BuscaQuery(Func<T, bool> predicate)
            => predicate != null ? _repositorioBase.BuscaQuery(predicate) : throw new ArgumentException("Parâmetro inválido");

        public T BuscarPorId(int id) 
            => id > 0 ? _repositorioBase.BuscarPorId(id) : throw new ArgumentException("Parâmetro inválido");

        public IEnumerable<T> BuscarTodos() => _repositorioBase.BuscarTodos();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
