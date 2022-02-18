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
        public override Usuario BuscarPorId(int id)
        {
            using (var c = new AntigueraContexto())
            {
                return c.Usuarios.AsNoTracking().Where(u => u.Id == id).FirstOrDefault();
            }
        }

        public override void Apagar(Usuario obj)
        {
            Context.Set<Usuario>().Attach(obj);
            base.Apagar(obj);
        }
    }
}
