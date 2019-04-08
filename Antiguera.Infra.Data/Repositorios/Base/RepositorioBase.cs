using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios.Base
{
    public class RepositorioBase<T> : IDisposable, IRepositorioBase<T> where T : class
    {
        protected DbContext Context { get; private set; }
        private IAntigueraContexto antigueraContexto { get; set; }

        public RepositorioBase()
        {
            Context = new AntigueraContexto();
        }
                
        public RepositorioBase(IAntigueraContexto antigueraContexto)
        {
            this.antigueraContexto = antigueraContexto;
        }

        public RepositorioBase(AntigueraContexto antigueraContexto)
        {
            Context = antigueraContexto;
        }

        public virtual void Adicionar(T obj)
        {
            Context.Set<T>().Add(obj);
            Context.SaveChanges();
        }

        public virtual void Apagar(T obj)
        {
            Context.Set<T>().Remove(obj);
            Context.SaveChanges();
        }

        public virtual void Atualizar(T obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual T BuscarPorId(int id) => Context.Set<T>().Find(id);

        public virtual IEnumerable<T> BuscarTodos() => Context.Set<T>().ToList();

        public virtual IEnumerable<T> BuscaQuery(Func<T, bool> predicate) => Context.Set<T>().Where(predicate);

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
