using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios
{
    public class AcessoRepositorio : RepositorioBase<Acesso>, IAcessoRepositorio
    {
        private AntigueraContexto _context;

        public AcessoRepositorio(AntigueraContexto context)
            :base(context)
        {
            _context = context;
        }

        public override void Atualizar(Acesso obj)
        {
            _context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public override void Apagar(Acesso obj)
        {
            var acesso = Context.Set<Acesso>().Local.FirstOrDefault(a => a.Id == obj.Id);
            if(acesso != null)
            {
                _context.Entry(acesso).State = System.Data.Entity.EntityState.Detached;
                _context.Set<Acesso>().Attach(acesso);
                base.Apagar(acesso);
            }
            else
            {
                _context.Set<Acesso>().Attach(obj);
                base.Apagar(obj);
            }
        }

        public Acesso BuscarPorIdentityRole(string identityRoleId)
        {
            return _context.Acessos.AsNoTracking().FirstOrDefault(u => u.IdentityRoleId == identityRoleId);
        }
    }
}
