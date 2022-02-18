using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class UsuarioConfiguracao : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguracao()
        {
            HasKey(u => u.Id).Property(u => u.Id);

            Property(u => u.IdentityUserId).IsRequired();
            
            Property(u => u.UltimaVisita).IsOptional();

            Property(u => u.Novo).IsOptional();

            Property(u => u.Created).IsOptional();

            Property(u => u.Modified).IsOptional();
        }
    }
}
