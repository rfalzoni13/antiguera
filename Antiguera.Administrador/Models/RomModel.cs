using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Antiguera.Administrador.Models
{
    public class RomModel
    {
        public int Id { get; set; }

        public int EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime DataLancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string UrlArquivo { get; set; }

        [DisplayName("Arquivo do programa")]
        [FileExtensions(Extensions = ".zip, .rar", ErrorMessage = "Somente são aceitos os tipos .zip e .rar")]
        public HttpPostedFileBase FileRom { get; set; }

        public string UrlBoxArt { get; set; }

        [DisplayName("Arte da capa")]
        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public HttpPostedFileBase FileBoxArt { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}