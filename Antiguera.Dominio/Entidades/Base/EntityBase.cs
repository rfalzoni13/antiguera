using Antiguera.Dominio.Interfaces.Entity;
using System;

namespace Antiguera.Dominio.Entidades.Base
{
    public class EntityBase : IEntity
    {
        public Guid Id { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
