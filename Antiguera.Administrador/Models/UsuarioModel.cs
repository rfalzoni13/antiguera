using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nível de acesso")]
        [Required(ErrorMessage = "O campo nível de acesso é obrigatório")]
        public int AcessoId { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo sexo é obrigatório")]
        public string Sexo { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo login é obrigatório")]
        public string Login { get; set; }

        [StringLength(12, ErrorMessage = "A senha deve ter entre 6 a 12 caracteres", MinimumLength = 6)]
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Senha { get; set; }

        [DisplayName("Confirmar senha")]
        [Required(ErrorMessage = "O campo de confirmação de senha é obrigatório")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "As senhas não conferem")]
        public string ConfirmarSenha { get; set; }

        public string UrlFotoUpload { get; set; }

        [DisplayName("Foto de perfil")]
        public HttpPostedFileBase FileFoto { get; set; }

        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public string NomeFoto
        {
            get
            {
                if (FileFoto != null && FileFoto.ContentLength > 0)
                {
                    return FileFoto.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public int? NumAcessos { get; set; }

        public int? NumDownloadsJogos { get; set; }

        public int? NumDownloadsProg { get; set; }

        public bool? Novo { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual AcessoModel Acesso { get; set; }

        public virtual List<SelectListItem> ListaAcessos { get; set; }

        public virtual List<SelectListItem> Sexos
        {
            get
            {
                var lista = new List<SelectListItem>();
                lista.Add(new SelectListItem() { Text = "Selecione uma opção...", Value = null });
                lista.Add(new SelectListItem() { Text = "Masculino", Value = "Masculino" });
                lista.Add(new SelectListItem() { Text = "Feminino", Value = "Feminino" });
                return lista;
            }
        }
    }
}