﻿using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IRomRepositorio : IRepositorioBase<Rom>
    {
        void ApagarRoms(int[] Ids);
    }
}