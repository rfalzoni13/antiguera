using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

namespace Antiguera.Infra.Data.Repositorios
{
    public class AcessoRepositorio : RepositorioBase<Acesso>, IAcessoRepositorio
    {
        public void ApagarAcessos(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if (id > 0)
                {
                    var acesso = Context.Set<Acesso>().Find(id);
                    if (acesso != null)
                    {
                        Context.Set<Acesso>().Remove(acesso);
                    }
                }
            }
            Context.SaveChanges();
        }
        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var acesso = c.Acessos.Where(u => u.Id == id).FirstOrDefault();
                if (acesso != null)
                {
                    acesso.Novo = false;
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
