using System;

namespace Antiguera.Dominio.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
