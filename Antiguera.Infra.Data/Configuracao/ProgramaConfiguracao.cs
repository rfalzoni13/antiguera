using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class ProgramaConfiguracao : EntityTypeConfiguration<Programa>
    {
        public ProgramaConfiguracao()
        {
            HasKey(p => p.Id).Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.Nome).HasColumnOrder(2).IsRequired().HasMaxLength(220);

            Property(p => p.Descricao).HasColumnOrder(3).IsRequired().HasColumnType("text");

            Property(p => p.Developer).HasColumnOrder(4).IsRequired().HasMaxLength(100);

            Property(p => p.Publisher).HasColumnOrder(5).IsRequired().HasMaxLength(100);

            Property(p => p.Lancamento).HasColumnOrder(6).IsRequired();

            Property(p => p.UrlBoxArt).HasMaxLength(200).HasColumnOrder(7).IsOptional();

            Property(p => p.UrlArquivo).HasMaxLength(200).HasColumnOrder(8).IsOptional();

            Property(p => p.TipoPrograma).HasColumnOrder(9).IsRequired().HasMaxLength(100);

            Property(p => p.Novo).IsOptional().HasColumnOrder(10);

            Property(p => p.Created).IsRequired().HasColumnOrder(11);

            Property(p => p.Modified).IsOptional().HasColumnOrder(12);
        }
    }
}
