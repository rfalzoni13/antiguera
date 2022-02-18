using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class HistoricoConfiguracao : EntityTypeConfiguration<Historico>
    {
        public HistoricoConfiguracao()
        {
            HasKey(h => h.Id).Property(u => u.Id);

            Property(h => h.UsuarioId).IsRequired();

            Property(h => h.Data).IsRequired();

            Property(h => h.Novo).IsOptional();

            Property(h => h.TipoHistorico).IsRequired();

            Property(h => h.Created).IsRequired();

            Property(h => h.Modified).IsOptional();

            HasRequired(h => h.Usuario).WithMany().HasForeignKey(h => h.UsuarioId).WillCascadeOnDelete();
        }
    }
}
