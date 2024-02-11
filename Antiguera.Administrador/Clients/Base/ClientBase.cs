using Antiguera.Administrador.Models;
using Antiguera.Utils.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients.Base
{
    public abstract class ClientBase<T, TTable> : IClientBase<T, TTable>
        where T : class
        where TTable : class
    {
        protected string token;

        public ClientBase()
        {
            token = RequestHelper.GetAccessToken();
        }

        public virtual async Task<string> Atualizar(string url, T obj)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.PutAsJsonAsync(url, obj);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public virtual async Task<string> Excluir(string url, T obj)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public virtual async Task<string> Inserir(string url, T obj)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.PostAsJsonAsync(url, obj);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public virtual async Task<T> Listar(string url, string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{url}?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public virtual async Task<TTable> ListarTabela(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TTable>();
                }
                else
                {
                    StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }
        public virtual async Task<ICollection<T>> ListarTodos(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ICollection<T>>();
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