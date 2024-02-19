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
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        public string Email { get; set; }

        [DisplayName("Gênero")]
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Genero { get; set; }

        [DisplayName("Data de nascimento")]
        public DateTime DataNascimento { get; set; }

        public string Login { get; set; }

        public string Telefone { get; set; }

        public string CaminhoFoto { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public string[] Acessos { get; set; }

        public MultiSelectList Perfis { get; set; }

        public List<SelectListItem> Sexos 
        {
            get
            {
                return new List<SelectListItem>
                (
                    new SelectListItem[]
                    {
                        new SelectListItem
                        {
                            Text = "Masculino",
                            Value = "Maculino"
                        },
                        new SelectListItem
                        {
                            Text = "Feminino",
                            Value = "Feminino"
                        }
                    }
                );
            }
        }

        public HttpPostedFileBase ArquivoPerfil { get; set; }
    }
}