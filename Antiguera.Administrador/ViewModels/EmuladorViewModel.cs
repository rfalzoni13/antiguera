using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Antiguera.Administrador.ViewModels
{
    public class EmuladorViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome do emulador")]
        [Required(ErrorMessage = "O nome do emulador é obrigatório!")]
        public string Nome { get; set; }

        [DisplayName("Data de lançamento")]
        [Required(ErrorMessage = "A data de lançamento é obrigatória!")]
        public DateTime Lancamento { get; set; }

        [DisplayName("Console")]
        [Required(ErrorMessage = "O campo console é obrigatório!")]
        public string Console { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória!")]
        public string Descricao { get; set; }

        public string Arquivo { get; set; }

        [DisplayName("Arquivo do programa")]
        public HttpPostedFileBase FileEmulador { get; set; }

        [FileExtensions(Extensions = ".zip, .rar", ErrorMessage = "Somente são aceitos os tipos .zip e .rar")]
        public string NomeArquivo
        {
            get
            {
                if (FileEmulador != null && FileEmulador.ContentLength > 0)
                {
                    return FileEmulador.FileName;
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

        public virtual List<RomViewModel> Roms { get; set; }
    }
}