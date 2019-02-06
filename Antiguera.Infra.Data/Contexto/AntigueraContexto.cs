using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Infra.Data.Configuracao;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Antiguera.Infra.Data.Contexto
{
    public class AntigueraContexto : DbContext, IAntigueraContexto
    {
        #region Atributos
        public virtual DbSet<Usuario> Usuarios { get; set; }

        public virtual DbSet<Programa> Programas { get; set; }

        public virtual DbSet<Jogo> Jogos { get; set; }

        public virtual DbSet<Emulador> Emuladores { get; set; }

        public virtual DbSet<Rom> Roms { get; set; }

        public virtual DbSet<Acesso> Acessos { get; set; }

        #region Interfaces
        IQueryable<Usuario> IAntigueraContexto.Usuarios => Usuarios;

        IQueryable<Programa> IAntigueraContexto.Programas => Programas;

        IQueryable<Jogo> IAntigueraContexto.Jogos => Jogos;

        IQueryable<Emulador> IAntigueraContexto.Emuladores => Emuladores;

        IQueryable<Rom> IAntigueraContexto.Roms => Roms;

        IQueryable<Acesso> IAntigueraContexto.Acessos => Acessos;
        #endregion
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

            modelBuilder.Configurations.Add(new UsuarioConfiguracao());
            modelBuilder.Configurations.Add(new JogoConfiguracao());
            modelBuilder.Configurations.Add(new EmuladorConfiguracao());
            modelBuilder.Configurations.Add(new RomConfiguracao());
            modelBuilder.Configurations.Add(new ProgramaConfiguracao());
            modelBuilder.Configurations.Add(new AcessoConfiguracao());

            modelBuilder.Entity<Rom>().HasRequired<Emulador>(r => r.Emulador).WithMany(e => e.Roms)
            .HasForeignKey(r => r.EmuladorId);
        }
    }
}
