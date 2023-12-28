﻿using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;

namespace Antiguera.Administrador.Clients.Interface
{
    public interface IJogoClient : IClientBase<JogoModel, JogoTableModel>
    {
    }
}
