using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Antiguera.Administrador.Clients.Interface
{
    public interface IAcessoClient : IClientBase<AcessoModel, AcessoTableModel>
    {
        MultiSelectList ObterTodosNomesAcessos();
    }
}