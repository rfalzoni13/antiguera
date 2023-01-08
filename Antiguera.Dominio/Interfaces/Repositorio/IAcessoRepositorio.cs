﻿using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IAcessoRepositorio : IRepositorioBase<Acesso>
    {
        Acesso BuscarPorIdentityRole(string identityRoleId);
    }
}
