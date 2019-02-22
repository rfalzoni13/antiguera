using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

namespace Antiguera.Infra.Data.Repositorios
{
    public class JogoRepositorio : RepositorioBase<Jogo>, IJogoRepositorio
    {
        public void ApagarJogos(int[] Ids)
        {
            foreach(var id in Ids)
            {
                if(id > 0)
                {
                    var jogo = Context.Set<Jogo>().Find(id);
                    if(jogo != null)
                    {
                        Context.Set<Jogo>().Remove(jogo);
                    }
                }
            }
            Context.SaveChanges();
        }

        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var jogo = c.Jogos.Where(u => u.Id == id).FirstOrDefault();
                if (jogo != null)
                {
                    jogo.Novo = false;
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
