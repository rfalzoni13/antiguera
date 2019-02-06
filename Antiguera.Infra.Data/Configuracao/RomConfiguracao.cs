using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class RomConfiguracao : EntityTypeConfiguration<Rom>
    {
        public RomConfiguracao()
        {
            HasKey(r => r.Id).Property(r => r.Id).HasColumnOrder(1);

            Property(r => r.EmuladorId).IsRequired().HasColumnOrder(2);

            Property(r => r.Nome).HasMaxLength(100).IsRequired().HasColumnOrder(3);

            Property(r => r.DataLancamento).IsRequired().HasColumnOrder(4);

            Property(r => r.Descricao).HasColumnType("text").IsRequired().HasColumnOrder(5);

            Property(r => r.Genero).HasMaxLength(50).IsRequired().HasColumnOrder(6);

            Property(r => r.Novo).IsOptional().HasColumnOrder(7);

            Property(r => r.Created).IsRequired().HasColumnOrder(8);

            Property(r => r.Modified).IsOptional().HasColumnOrder(9);
        }
    }
}
