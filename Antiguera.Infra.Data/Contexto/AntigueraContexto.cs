using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Infra.Data.Configuracao;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Antiguera.Infra.Data.Contexto
{
    public class AntigueraContexto : DbContext
    {
        #region Atributos

        public virtual DbSet<Emulador> Emuladores { get; set; }

        public virtual DbSet<Jogo> Jogos { get; set; }

        public virtual DbSet<Programa> Programas { get; set; }

        public virtual DbSet<Rom> Roms { get; set; }
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

            modelBuilder.Configurations.Add(new EmuladorConfiguracao());
            modelBuilder.Configurations.Add(new HistoricoConfiguracao());
            modelBuilder.Configurations.Add(new JogoConfiguracao());
            modelBuilder.Configurations.Add(new ProgramaConfiguracao());
            modelBuilder.Configurations.Add(new RomConfiguracao());
        }
    }
}
