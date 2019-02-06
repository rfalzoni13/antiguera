﻿using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IRomServico : IServicoBase<Rom>
    {
        void ApagarRoms(int[] Ids);
    }
}