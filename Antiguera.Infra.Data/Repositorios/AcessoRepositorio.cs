using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;

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
        }
    }
}
