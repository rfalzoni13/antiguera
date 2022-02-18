using System;
using System.ComponentModel.DataAnnotations;

namespace Antiguera.Administrador.ViewModels
{
    public class AcessoViewModel
    {
        public int Id { get; set; }

        public string IdentityRoleId { get; set; }

        [Required(ErrorMessage = "O Nome do acesso é obrigatório!")]
        public string Nome { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}