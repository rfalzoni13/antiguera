using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class EmuladorConfiguracao : EntityTypeConfiguration<Emulador>
    {
        public EmuladorConfiguracao()
        {
            HasKey(e => e.Id).Property(e => e.Id);

            Property(e => e.Nome).IsRequired();

            Property(e => e.Lancamento).IsRequired();

            Property(e => e.Descricao).HasColumnType("text").IsRequired();

            Property(e => e.Console).IsRequired();

            Property(e => e.nomeArquivo).IsOptional();

            Property(e => e.hashArquivo).IsOptional();

            Property(e => e.Novo).IsOptional();

            Property(e => e.Created).IsRequired();

            Property(e => e.Modified).IsOptional();

            HasMany(e => e.Roms).WithRequired()
                .HasForeignKey(r => r.EmuladorId).WillCascadeOnDelete();
        }
    }
}
