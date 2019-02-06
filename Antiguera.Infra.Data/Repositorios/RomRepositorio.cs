using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System;

namespace Antiguera.Infra.Data.Repositorios
{
    public class RomRepositorio : RepositorioBase<Rom>, IRomRepositorio
    {
        public void ApagarRoms(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if (id > 0)
                {
                    var rom = Context.Set<Rom>().Find(id);
                    if (rom != null)
                    {
                        Context.Set<Rom>().Remove(rom);
                    }
                }
            }
        }
    }
}
