﻿using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico : IServicoBase<UsuarioDTO, Usuario>
    {
        UsuarioDTO ListarPorIdentityId(string userId);
    }
}
