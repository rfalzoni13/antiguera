using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class AcessoConfiguracao : EntityTypeConfiguration<Acesso>
    {
        public AcessoConfiguracao()
        {
            HasKey(a => a.Id).Property(a => a.Id);

            Property(a => a.IdentityRoleId).IsRequired();

            Property(a => a.Nome).IsRequired();

            Property(a => a.Novo).IsOptional();

            Property(a => a.Created).IsRequired();

            Property(a => a.Modified).IsOptional();
        }
    }
}
