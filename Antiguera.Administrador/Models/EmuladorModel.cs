﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Antiguera.Administrador.Models
{
    public class EmuladorModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome do emulador")]
        [Required(ErrorMessage = "O nome do emulador é obrigatório!")]
        public string Nome { get; set; }

        [DisplayName("Data de lançamento")]
        [Required(ErrorMessage = "A data de lançamento é obrigatória!")]
        public DateTime DataLancamento { get; set; }

        [DisplayName("Console")]
        [Required(ErrorMessage = "O campo console é obrigatório!")]
        public string Console { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória!")]
        public string Descricao { get; set; }

        public string UrlArquivo { get; set; }

        [DisplayName("Arquivo do programa")]
        [FileExtensions(Extensions = ".zip, .rar", ErrorMessage = "Somente são aceitos os tipos .zip e .rar")]
        public HttpPostedFileBase FileEmulador { get; set; }

        public string UrlBoxArt { get; set; }

        [DisplayName("Arte da capa")]
        [FileExtensions(Extensions = ".jpg, .png", ErrorMessage = "Somente são aceitos os tipos .jpg e .png")]
        public HttpPostedFileBase FileBoxArt { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual List<RomModel> Roms { get; set; }
    }
}