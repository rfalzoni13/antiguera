using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class JogoConfiguracao : EntityTypeConfiguration<Jogo>
    {
        public JogoConfiguracao()
        {
            HasKey(j => j.Id).Property(j => j.Id).HasColumnOrder(1);

            Property(j => j.Nome).HasColumnOrder(2).IsRequired().HasMaxLength(100);

            Property(j => j.Descricao).HasColumnOrder(3).IsRequired().HasColumnType("text");

            Property(j => j.Developer).HasColumnOrder(4).HasMaxLength(100).IsRequired();

            Property(j => j.Publisher).HasColumnOrder(5).HasMaxLength(100).IsRequired();

            Property(j => j.Lancamento).HasColumnOrder(6).IsRequired();

            Property(j => j.Plataforma).HasColumnOrder(7).IsRequired().HasMaxLength(100);

            Property(j => j.Genero).HasColumnOrder(8).IsRequired().HasMaxLength(50);

            Property(j => j.UrlBoxArt).HasColumnOrder(9).IsOptional().HasMaxLength(200);

            Property(j => j.UrlArquivo).HasColumnOrder(10).IsOptional().HasMaxLength(200);

            Property(j => j.Created).IsRequired().HasColumnOrder(11);

            Property(j => j.Modified).IsOptional().HasColumnOrder(12);
        }
    }
}
