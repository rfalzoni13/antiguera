using System;

namespace Antiguera.Dominio.Entidades
{
    public class Acesso
    {
        public int Id { get; set; }

        public string IdentityRoleId { get; set; }

        public string Nome { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}