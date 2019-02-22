using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

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
            Context.SaveChanges();
        }

        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var rom = c.Roms.Where(u => u.Id == id).FirstOrDefault();
                if (rom != null)
                {
                    rom.Novo = false;
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
