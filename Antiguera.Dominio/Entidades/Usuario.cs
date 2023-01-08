using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades.Base;
using System;

namespace Antiguera.Dominio.Entidades
{
    public class Usuario : EntityBase
    {
        public Guid AcessoId { get; set; }

        public string Nome { get; set; }

        public string IdentityUserId { get; set; }

        public string PathFoto { get; set; }

        public string Genero { get; set; }

        public string Telefone { get; set; }

        public DateTime DataNascimento { get; set; }

        public virtual Acesso Acesso { get; set; }

        public static Usuario ConvertToEntity(UsuarioDTO usuarioDTO, bool withAcesso = false)
        {
            return new Usuario
            {
                Id = usuarioDTO.Id,
                AcessoId = usuarioDTO.AcessoId,
                IdentityUserId = usuarioDTO.IdentityUserId,
                Nome = usuarioDTO.Nome,
                PathFoto = usuarioDTO.PathFoto,
                Created = usuarioDTO.Created,
                Telefone = usuarioDTO.Telefone,
                Modified = usuarioDTO.Modified,
                Novo = usuarioDTO.Novo,
                Acesso = withAcesso ? new Acesso
                {
                    Id = usuarioDTO.Acesso.Id,
                    IdentityRoleId = usuarioDTO.Acesso.IdentityRoleId,
                    Nome = usuarioDTO.Acesso.Nome,
                    Created = usuarioDTO.Acesso.Created,
                    Modified = usuarioDTO.Acesso.Modified,
                    Novo = usuarioDTO.Acesso.Novo
                }
                : null
            };
        }

    }
}