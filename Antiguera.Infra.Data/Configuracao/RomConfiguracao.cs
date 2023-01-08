using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class RomConfiguracao : EntityTypeConfiguration<Rom>
    {
        public RomConfiguracao()
        {
            HasKey(r => r.Id).Property(r => r.Id);

            Property(r => r.EmuladorId).IsRequired();

            Property(r => r.Nome).IsRequired();

            Property(r => r.Lancamento).IsRequired();

            Property(r => r.Descricao).HasColumnType("text").IsRequired();

            Property(r => r.Genero).IsRequired();

            Property(e => e.NomeArquivo).IsOptional();

            Property(e => e.HashArquivo).IsOptional();

            Property(e => e.BoxArt).IsOptional();

            Property(r => r.Novo).IsOptional();

            Property(r => r.Created).IsRequired();

            Property(r => r.Modified).IsOptional();
        }
    }
}
