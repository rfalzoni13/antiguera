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

            Property(u => u.Genero).IsRequired();

            Property(u => u.DataNascimento).IsRequired();

            Property(u => u.Telefone).IsOptional();

            Property(u => u.Novo).IsOptional();

            Property(u => u.Created).IsRequired();

            Property(u => u.Modified).IsOptional();
        }
    }
}
