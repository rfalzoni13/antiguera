using Antiguera.Dominio.Entidades.Base;

namespace Antiguera.Dominio.Entidades
{
    public class Acesso : EntityBase
    {
        public string Nome { get; set; }

        public string IdentityRoleId { get; set; }
    }
}