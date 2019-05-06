using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class AcessoConfiguracao : EntityTypeConfiguration<Acesso>
    {
        public AcessoConfiguracao()
        {
            HasKey(a => a.Id).Property(a => a.Id).HasColumnOrder(1);

            Property(a => a.IdentityRoleId).IsRequired().HasColumnOrder(2);

            Property(a => a.Nome).HasMaxLength(100).IsRequired().HasColumnOrder(3);

            Property(a => a.Novo).IsOptional().HasColumnOrder(4);

            Property(a => a.Created).IsRequired().HasColumnOrder(5);

            Property(a => a.Modified).IsOptional().HasColumnOrder(6);
        }
    }
}
