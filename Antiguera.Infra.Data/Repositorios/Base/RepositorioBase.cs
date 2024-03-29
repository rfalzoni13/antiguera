﻿using Antiguera.Dominio.Interfaces.Entity;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios.Base
{
    public class RepositorioBase<T> : IDisposable, IRepositorioBase<T> where T : class, IEntity
    {
        protected DbContext Context { get; private set; }

        public RepositorioBase()
        {
            Context = new AntigueraContexto();
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
            Context.Set<T>().Attach(obj);
            Context.Entry(obj).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual T BuscarPorId(Guid id) => Context.Set<T>().Find(id);

        public virtual IEnumerable<T> ListarTodos() => Context.Set<T>().ToList();

        public virtual IEnumerable<T> ListarPorPesquisa(Func<T, bool> predicate) => Context.Set<T>().Where(predicate);
        
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
