using System;
using System.Collections.Generic;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Servicos.Base
{
    public class ServicoBase<T> : IServicoBase<T> where T : class
    {
        #region Atributos
        private readonly IRepositorioBase<T> _repositorioBase;
        #endregion

        #region Construtor
        public ServicoBase(IRepositorioBase<T> repositorioBase)
        {
            _repositorioBase = repositorioBase;
        }
        #endregion

        public void Adicionar(T obj)
        {
            _repositorioBase.Adicionar(obj);
        }

        public void Apagar(T obj)
        {
            _repositorioBase.Apagar(obj);
        }

        public void Atualizar(T obj)
        {
            _repositorioBase.Atualizar(obj);
        }

        public T BuscarPorId(int id) => _repositorioBase.BuscarPorId(id);

        public IEnumerable<T> BuscarTodos() => _repositorioBase.BuscarTodos();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
