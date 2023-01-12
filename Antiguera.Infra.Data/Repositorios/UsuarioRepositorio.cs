using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System;
using System.Data.Entity;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        private AntigueraContexto _context;

        public UsuarioRepositorio(AntigueraContexto context)
            : base(context)
        {
            _context = context;
        }

        public override Usuario BuscarPorId(Guid id)
        {
            return _context.Usuarios.AsNoTracking().Where(u => u.Id == id).FirstOrDefault();
        }

        public Usuario BuscarPorIdentityId(string identityId)
            => _context.Set<Usuario>().Include(x => x.Acesso)
            .Where(x => x.IdentityUserId == identityId).FirstOrDefault();

        public override void Apagar(Usuario obj)
        {
            _context.Set<Usuario>().Attach(obj);
            base.Apagar(obj);
        }
    }
}
