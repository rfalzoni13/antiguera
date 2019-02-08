using Antiguera.Administrador.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace Antiguera.Administrador.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public int AcessoId { get; set; }

        public string Nome { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string UrlFotoUpload { get; set; }

        public int? NumAcessos { get; set; }

        public int? NumDownloadsJogos { get; set; }

        public int? NumDownloadsProg { get; set; }

        public bool? Novo { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual AcessoModel Acesso { get; set; }
    }
}