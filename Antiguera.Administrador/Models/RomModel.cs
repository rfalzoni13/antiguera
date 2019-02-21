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
        public HttpPostedFileBase FileRom { get; set; }

        [FileExtensions(Extensions = ".zip, .rar", ErrorMessage = "Somente são aceitos os tipos .zip e .rar")]
        public string NomeArquivo
        {
            get
            {
                if (FileRom != null && FileRom.ContentLength > 0)
                {
                    return FileRom.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public string UrlBoxArt { get; set; }

        [DisplayName("Arte da capa")]
        public HttpPostedFileBase FileBoxArt { get; set; }

        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public string NomeFoto
        {
            get
            {
                if (FileBoxArt != null && FileBoxArt.ContentLength > 0)
                {
                    return FileBoxArt.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}