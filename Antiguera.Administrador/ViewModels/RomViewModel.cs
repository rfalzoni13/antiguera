using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Antiguera.Administrador.ViewModels
{
    public class RomViewModel
    {
        public int Id { get; set; }

        public int EmuladorId { get; set; }

        [DisplayName("Nome da rom")]
        [Required(ErrorMessage = "O nome da rom é obrigatório!")]
        public string Nome { get; set; }

        [DisplayName("Data de lançamento")]
        [Required(ErrorMessage = "A data de lançamento é obrigatória!")]
        public DateTime Lancamento { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória!")]
        public string Descricao { get; set; }

        [DisplayName("Gênero")]
        [Required(ErrorMessage = "O gênero do jogo é obrigatório!")]
        public string Genero { get; set; }

        public string Arquivo { get; set; }

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

        public string BoxArt { get; set; }

        [DisplayName("Arte da capa")]
        public HttpPostedFileBase FileBoxArt { get; set; }

        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public string NomeBoxArt
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