using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Antiguera.Administrador.Models
{
    public class JogoModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome do jogo")]
        [Required(ErrorMessage = "O nome do jogo é obrigatório!")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória!")]
        public string Descricao { get; set; }

        [DisplayName("Data de lançamento")]
        [Required(ErrorMessage = "A data de lançamento é obrigatória!")]
        public DateTime Lancamento { get; set; }

        [DisplayName("Publicador")]
        [Required(ErrorMessage = "O nome do publicador é obrigatório!")]
        public string Publisher { get; set; }

        [DisplayName("Desenvolvedora")]
        [Required(ErrorMessage = "O nome da desenvolvedora é obrigatório!")]
        public string Developer { get; set; }

        [DisplayName("Gênero")]
        [Required(ErrorMessage = "O gênero do jogo é obrigatório!")]
        public string Genero { get; set; }

        public string UrlBoxArt { get; set; }

        [DisplayName("Arte da capa")]
        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public HttpPostedFileBase FileBoxArt { get; set; }

        public string UrlArquivo { get; set; }

        [DisplayName("Arquivo do jogo")]
        [FileExtensions(Extensions = ".zip, .rar", ErrorMessage = "Somente são aceitos os tipos .zip e .rar")]
        public HttpPostedFileBase FileJogo { get; set; }

        public bool? Novo { get; set; }

        [DisplayName("Plataforma")]
        [Required(ErrorMessage = "O nome da plataforma do jogo é obrigatório!")]
        public string Plataforma { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}