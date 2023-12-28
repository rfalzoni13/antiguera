using Antiguera.Administrador.Clients.Base;
using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients
{
    public class JogoClient : ClientBase<JogoModel, JogoTableModel>, IJogoClient
    {
        public async override Task<JogoTableModel> ListarTabela(string url)
        {
            var table = new JogoTableModel();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jogos = await response.Content.ReadAsAsync<ICollection<JogoModel>>();

                        foreach (var jogo in jogos)
                        {
                            table.data.Add(new JogoListTableModel()
                            {
                                Id = jogo.Id,
                                Nome = jogo.Nome,
                                Developer = jogo.Developer,
                                Publisher = jogo.Publisher,
                                Genero = jogo.Genero,
                                Plataforma = jogo.Plataforma,
                                Created = jogo.Created,
                                Modified = jogo.Modified,
                                Novo = jogo.Novo
                            });
                        }

                        table.recordsFiltered = table.data.Count();
                        table.recordsTotal = table.data.Count();
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        if(statusCode.Status != System.Net.HttpStatusCode.NotFound)
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

        public override async Task<string> Inserir(string url, JogoModel obj)
        {
            using(var client = new RestClient(new RestClientOptions { 
                ThrowOnAnyError = true
            }))
            {
                var request = new RestRequest(url, Method.Post);

                request.AddHeader("Authorization", $"Bearer {token}");

                request.AddBody(obj);

                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content;
                }
                else
                {
                    StatusCodeModel statusCode = JsonConvert.DeserializeObject<StatusCodeModel>(response.Content);

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }
    }
}