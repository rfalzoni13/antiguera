using Antiguera.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace Antiguera.Infra.Data.Configuracao
{
    public class UsuarioConfiguracao : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguracao()
        {
            HasKey(u => u.Id).Property(u => u.Id).HasColumnOrder(1);

            Property(u => u.AcessoId).IsRequired().HasColumnOrder(2);

            Property(u => u.Nome).HasColumnOrder(3).IsRequired().HasMaxLength(220);

            Property(u => u.Email).HasColumnOrder(4).IsRequired().HasMaxLength(220);

            Property(u => u.Sexo).HasColumnOrder(5).IsRequired().HasMaxLength(10);

            Property(u => u.Login).HasColumnOrder(6).IsRequired().HasMaxLength(30);

            Property(u => u.Senha).HasColumnOrder(7).IsRequired().HasMaxLength(220);

            Property(u => u.NumAcessos).HasColumnOrder(8).IsOptional();

            Property(u => u.NumDownloadsJogos).HasColumnOrder(9).IsOptional();

            Property(u => u.NumDownloadsProg).HasColumnOrder(10).IsOptional();
            
            Property(u => u.UltimaVisita).HasColumnOrder(11).IsOptional();

            Property(u => u.UrlFotoUpload).HasColumnOrder(12).IsOptional().HasMaxLength(200);

            Property(u => u.Created).HasColumnOrder(13).IsOptional();

            Property(u => u.Modified).HasColumnOrder(14).IsOptional();

            HasRequired(u => u.Acesso).WithMany().HasForeignKey(u => u.AcessoId).WillCascadeOnDelete();
        }
    }
}
