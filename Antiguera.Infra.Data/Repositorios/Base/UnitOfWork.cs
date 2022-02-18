using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Infra.Data.Contexto;
using System.Data.Entity;

namespace Antiguera.Infra.Data.Repositorios.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AntigueraContexto _context;

        public UnitOfWork(AntigueraContexto context)
        {
            _context = context;
        }


        public DbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.BeginTransaction().Commit();
        }

        public void RollBack()
        {
            _context.Database.BeginTransaction().Rollback();
        }

        public void Dispose()
        {
            _context.Database.BeginTransaction().Dispose();
        }
    }
}
