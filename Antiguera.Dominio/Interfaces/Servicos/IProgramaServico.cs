﻿using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos.Base;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IProgramaServico : IServicoBase<Programa>
    {
        void AtualizarNovo(int id);

        void ApagarProgramas(int[] Ids);
    }
}
