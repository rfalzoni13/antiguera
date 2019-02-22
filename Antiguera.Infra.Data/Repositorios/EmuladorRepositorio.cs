using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

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
            Context.SaveChanges();
        }

        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var emulador = c.Emuladores.Where(u => u.Id == id).FirstOrDefault();
                if (emulador != null)
                {
                    emulador.Novo = false;
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
