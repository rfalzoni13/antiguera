using Antiguera.Dominio.Entidades;
using System;
using System.Linq;

namespace Antiguera.Dominio.Interfaces
{
    public interface IAntigueraContexto : IDisposable
    {
        IQueryable<Usuario> Usuarios { get; }

        IQueryable<Programa> Programas { get; }

        IQueryable<Jogo> Jogos { get; }

        IQueryable<Emulador> Emuladores { get; }

        IQueryable<Rom> Roms { get; }

        IQueryable<Acesso> Acessos { get; }

    }
}
