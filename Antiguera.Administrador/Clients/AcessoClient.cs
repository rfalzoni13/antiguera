using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients
{
    public class AcessoClient : ClientBase<AcessoModel, AcessoTableModel>, IAcessoClient
    {
        public async override Task<AcessoTableModel> ListarTabela (string url)
        {
            var table = new AcessoTableModel();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var acessos = await response.Content.ReadAsAsync<ICollection<AcessoModel>>();

                        foreach (var acesso in acessos)
                        {
                            table.data.Add(new AcessoListTableModel()
                            {
                                Id = acesso.Id,
                                Nome = acesso.Nome,
                                Created = acesso.Created,
                                Modified = acesso.Modified,
                                Novo = acesso.Novo
                            });
                        }

                        table.recordsFiltered = table.data.Count();
                        table.recordsTotal = table.data.Count();
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        table.error = statusCode.Message;
                    }
                }
            }
            catch(Exception ex)
            {
                table.error = ExceptionHelper.CatchMessageFromException(ex);
            }

            return await Task.FromResult(table);
        }
    }
}