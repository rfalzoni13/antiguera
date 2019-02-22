using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

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
            Context.SaveChanges();
        }

        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var programa = c.Programas.Where(u => u.Id == id).FirstOrDefault();
                if (programa != null)
                {
                    programa.Novo = false;
                    c.SaveChanges();
                }
                else
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }
            }
        }
    }
}
