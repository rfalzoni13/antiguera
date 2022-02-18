using Antiguera.Dominio.DTO.Base;

namespace Antiguera.Dominio.DTO
{
    public class AcessoDTO : BaseDTO
    {
        public AcessoDTO()
        {
        }

        public string Nome { get; set; }

        public string IdentityRoleId { get; set; }
    }
}
