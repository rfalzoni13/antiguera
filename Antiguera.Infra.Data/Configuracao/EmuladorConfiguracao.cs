using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class EmuladorConfiguracao : EntityTypeConfiguration<Emulador>
    {
        public EmuladorConfiguracao()
        {
            HasKey(e => e.Id).Property(e => e.Id).HasColumnOrder(1);

            Property(e => e.Nome).IsRequired().HasMaxLength(100).HasColumnOrder(2);

            Property(e => e.DataLancamento).IsRequired().HasColumnOrder(3);

            Property(e => e.Descricao).HasColumnType("text").IsRequired().HasColumnOrder(4);

            Property(e => e.Console).HasMaxLength(50).IsRequired().HasColumnOrder(5);

            Property(e => e.UrlArquivo).HasMaxLength(200).IsOptional().HasColumnOrder(6);

            Property(e => e.Novo).IsOptional().HasColumnOrder(7);

            Property(e => e.Created).IsRequired().HasColumnOrder(8);

            Property(e => e.Modified).IsOptional().HasColumnOrder(9);
        }
    }
}
