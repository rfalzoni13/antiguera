using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Antiguera.Infra.Data.Contexto
{
    public class AntigueraContexto : DbContext
    {
        #region Atributos
        #endregion

        public AntigueraContexto()
            :base("Antiguera")
        {
            this.Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
