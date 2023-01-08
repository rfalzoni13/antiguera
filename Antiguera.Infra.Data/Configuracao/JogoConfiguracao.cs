using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class JogoConfiguracao : EntityTypeConfiguration<Jogo>
    {
        public JogoConfiguracao()
        {
            HasKey(j => j.Id).Property(j => j.Id);

            Property(j => j.Nome).IsRequired();

            Property(j => j.Descricao).IsRequired().HasColumnType("text");

            Property(j => j.Developer).IsRequired();

            Property(j => j.Publisher).IsRequired();

            Property(j => j.Lancamento).IsRequired();

            Property(j => j.Plataforma).IsRequired();

            Property(j => j.Genero).IsRequired();

            Property(j => j.BoxArt).IsOptional();

            Property(j => j.NomeArquivo).IsOptional();

            Property(j => j.HashArquivo).IsOptional();

            Property(j => j.Novo).IsRequired();

            Property(j => j.Created).IsRequired();

            Property(j => j.Modified).IsOptional();
        }
    }
}
