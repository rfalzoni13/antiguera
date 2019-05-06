using System;

namespace Antiguera.Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public int AcessoId { get; set; }

        public string IdentityUserId { get; set; }

        public string Nome { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string UrlFotoUpload { get; set; }

        public int? NumAcessos { get; set; }

        public int? NumDownloadsJogos { get; set; }

        public int? NumDownloadsProg { get; set; }

        public bool? Novo { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual Acesso Acesso { get; set; }
    }
}