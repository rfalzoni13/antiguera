﻿using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio.Base;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IProgramaRepositorio : IRepositorioBase<Programa>
    {
        void ApagarProgramas(int[] Ids);
    }
}