using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System;

namespace Antiguera.Infra.Data.Repositorios
{
    public class EmuladorRepositorio : RepositorioBase<Emulador>, IEmuladorRepositorio
    {
        public void ApagarEmuladores(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if (id > 0)
                {
                    var emulador = Context.Set<Emulador>().Find(id);
                    if (emulador != null)
                    {
                        Context.Set<Emulador>().Remove(emulador);
                    }
                }
            }
        }
    }
}
