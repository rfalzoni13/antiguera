using System;

namespace Antiguera.Administrador.Models
{
    public class AcessoModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}