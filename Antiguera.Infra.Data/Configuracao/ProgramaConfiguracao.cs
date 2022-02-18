using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class ProgramaConfiguracao : EntityTypeConfiguration<Programa>
    {
        public ProgramaConfiguracao()
        {
            HasKey(p => p.Id).Property(p => p.Id);

            Property(p => p.Nome).IsRequired();

            Property(p => p.Descricao).IsRequired().HasColumnType("text");

            Property(p => p.Developer).IsRequired();

            Property(p => p.Publisher).IsRequired();

            Property(p => p.Lancamento).IsRequired();

            Property(p => p.BoxArt).IsOptional();

            Property(p => p.nomeArquivo).IsOptional();

            Property(p => p.hashArquivo).IsOptional();

            Property(p => p.Tipo).IsRequired();

            Property(p => p.Novo).IsOptional();

            Property(p => p.Created).IsRequired();

            Property(p => p.Modified).IsOptional();
        }
    }
}
