using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Dominio.Interfaces.Servicos.Base;
using System.Collections.Generic;

namespace Antiguera.Aplicacao.Servicos.Base
{
    public class AppServicoBase<T> : IAppServicoBase<T> where T : class
    {
        private IServicoBase<T> _servicoBase;

        public AppServicoBase(IServicoBase<T> servicoBase)
        {
            _servicoBase = servicoBase;
        }

        public void Adicionar(T obj)
        {
            _servicoBase.Adicionar(obj);
        }

        public void Apagar(T obj)
        {
            _servicoBase.Apagar(obj);
        }

        public void Atualizar(T obj)
        {
            _servicoBase.Atualizar(obj);
        }

        public T BuscarPorId(int id) => _servicoBase.BuscarPorId(id);

        public IEnumerable<T> BuscarTodos() => _servicoBase.BuscarTodos();

        public void Dispose()
        {
            _servicoBase.Dispose();
        }
    }
}
