using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using System.Linq;
using System.Web.Http;

namespace Antiguera.Infra.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        public void AlterarSenha(int id, string senha)
        {
            using (var c = new AntigueraContexto())
            {
                var usuario = c.Usuarios.Where(u => u.Id == id).FirstOrDefault();
                if(usuario != null)
                {
                    senha = BCrypt.HashPassword(senha, BCrypt.GenerateSalt());
                    usuario.Senha = senha;
                    c.SaveChanges();
                }
                else
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }
            }
        }

        public void ApagarUsuarios(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if (id > 0)
                {
                    var usuario = Context.Set<Usuario>().Find(id);
                    if (usuario != null)
                    {
                        Context.Set<Usuario>().Remove(usuario);
                    }
                }
            }
            Context.SaveChanges();
        }

        public void AtualizarNovo(int id)
        {
            using (var c = new AntigueraContexto())
            {
                var usuario = c.Usuarios.Where(u => u.Id == id).FirstOrDefault();
                if (usuario != null)
                {
                    usuario.Novo = false;
                    c.SaveChanges();
                }
                else
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }
            }
        }

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
