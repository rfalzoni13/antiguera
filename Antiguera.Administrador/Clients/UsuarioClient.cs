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
    public class UsuarioClient : ClientBase<UsuarioModel, UsuarioTableModel>, IUsuarioClient
    {
        public UsuarioClient() :base() { }

        public async override Task<UsuarioTableModel> ListarTabela(string url)
        {
            var table = new UsuarioTableModel();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var usuarios = await response.Content.ReadAsAsync<ICollection<UsuarioModel>>();

                        foreach (var usuario in usuarios)
                        {
                            table.data.Add(new UsuarioListTableModel()
                            {
                                Id = usuario.Id,
                                Nome = usuario.Nome,
                                Email = usuario.Email,
                                Login = usuario.Login,
                                Sexo = usuario.Genero,
                                Created = usuario.Created,
                                Modified = usuario.Modified,
                                Novo = usuario.Novo
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