using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios
{
    public class RomRepositorio : RepositorioBase<Rom>, IRomRepositorio
    {
        public override IEnumerable<Rom> BuscaQuery(Func<Rom, bool> predicate)
            => Context.Set<Rom>().Include(x => x.Emulador).Where(predicate);
    }
}
