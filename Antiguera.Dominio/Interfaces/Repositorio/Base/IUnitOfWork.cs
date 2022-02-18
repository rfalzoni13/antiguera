using System;
using System.Data.Entity;

namespace Antiguera.Dominio.Interfaces.Repositorio.Base
{
    public interface IUnitOfWork : IDisposable
    {
        DbContextTransaction BeginTransaction();

        void Commit();

        void RollBack();
    }
}
