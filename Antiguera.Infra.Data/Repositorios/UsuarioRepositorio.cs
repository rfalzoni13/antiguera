using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Antiguera.Infra.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        public override IEnumerable<Usuario> BuscaQuery(Func<Usuario, bool> predicate)
            => Context.Set<Usuario>().Include(x => x.Acesso).Where(predicate);

        public Usuario BuscarUsuarioPorLoginOuEmail(string data)
        {
            using (var c = new AntigueraContexto())
            {
                var usuario = c.Usuarios.Where(u => u.Login == data || u.Email == data).FirstOrDefault();
                if(usuario != null)
                {
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
