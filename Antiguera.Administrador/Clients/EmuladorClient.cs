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
    public class EmuladorClient : ClientBase<EmuladorModel, EmuladorTableModel>, IEmuladorClient
    {
        public async override Task<EmuladorTableModel> ListarTabela(string url)
        {
            var table = new EmuladorTableModel();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var emuladores = await response.Content.ReadAsAsync<ICollection<EmuladorModel>>();

                        foreach (var emulador in emuladores)
                        {
                            table.data.Add(new EmuladorListTableModel()
                            {
                                Id = emulador.Id,
                                Nome = emulador.Nome,
                                Console = emulador.Console,
                                Roms = emulador.Roms.Count(),
                                Created = emulador.Created,
                                Modified = emulador.Modified,
                                Novo = emulador.Novo
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