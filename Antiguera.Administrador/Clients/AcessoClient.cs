using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Common;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace Antiguera.Administrador.Clients
{
    public class AcessoClient : ClientBase<AcessoModel, AcessoTableModel>, IAcessoClient
    {
        public AcessoClient() :base() { }

        public MultiSelectList ObterTodosNomesAcessos()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = UrlConfigurationHelper.AcessoGetAllNames;

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var acessos = response.Content.ReadAsAsync<List<string>>().Result;

                    return new MultiSelectList(acessos);
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }
    }
}