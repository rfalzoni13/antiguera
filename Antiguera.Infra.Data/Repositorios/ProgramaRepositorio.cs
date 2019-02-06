using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System;

namespace Antiguera.Infra.Data.Repositorios
{
    public class ProgramaRepositorio : RepositorioBase<Programa>, IProgramaRepositorio
    {
        public void ApagarProgramas(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if (id > 0)
                {
                    var programa = Context.Set<Programa>().Find(id);
                    if (programa != null)
                    {
                        Context.Set<Programa>().Remove(programa);
                    }
                }
            }
        }
    }
}
