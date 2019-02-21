using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System;

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
        }

        public override void Apagar(Jogo obj)
        {
            using (var c = new AntigueraContexto())
            {
                c.Jogos.Attach(obj);
                c.Jogos.Remove(obj);
                c.SaveChanges();
            }
        }
    }
}
