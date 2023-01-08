using System;

namespace Antiguera.Dominio.DTO.Base
{
    public class BaseDTO
    {
        public Guid Id { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
