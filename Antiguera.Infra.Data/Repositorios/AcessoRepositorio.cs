using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios
{
    public class AcessoRepositorio : RepositorioBase<Acesso>, IAcessoRepositorio
    {
        public override void Atualizar(Acesso obj)
        {
            Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();
        }

        public override void Apagar(Acesso obj)
        {
            var acesso = Context.Set<Acesso>().Local.FirstOrDefault(a => a.Id == obj.Id);
            if(acesso != null)
            {
                Context.Entry(acesso).State = System.Data.Entity.EntityState.Detached;
                Context.Set<Acesso>().Attach(acesso);
                base.Apagar(acesso);
            }
            else
            {
                Context.Set<Acesso>().Attach(obj);
                base.Apagar(obj);
            }
        }
    }
}
