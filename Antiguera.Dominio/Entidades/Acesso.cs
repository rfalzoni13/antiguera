using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;

namespace Antiguera.Dominio.Entidades
{
    public class Acesso : EntityBase
    {
        public string Nome { get; set; }

        public string IdentityRoleId { get; set; }

        public static Acesso ConvertToEntity(AcessoDTO acessoDTO)
        {
            return new Acesso
            {
                Id = acessoDTO.Id,
                IdentityRoleId = acessoDTO.IdentityRoleId,
                Nome = acessoDTO.Nome,
                Created = acessoDTO.Created,
                Modified = acessoDTO.Modified,
                Novo = acessoDTO.Novo
            };
        }
    }
}