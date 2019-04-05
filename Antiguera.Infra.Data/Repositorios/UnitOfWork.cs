using Antiguera.Dominio.Interfaces;
using Antiguera.Infra.Data.Contexto;
using System;

namespace Antiguera.Infra.Data.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AntigueraContexto _antigueraContexto;
        private bool _disposed = false;

        public UnitOfWork(AntigueraContexto antigueraContexto)
        {
            _antigueraContexto = antigueraContexto;
        }

        public void Commit()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            _antigueraContexto.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing && _antigueraContexto != null)
            {
                _antigueraContexto.Dispose();
            }

            _disposed = true;
        }
    }
}
