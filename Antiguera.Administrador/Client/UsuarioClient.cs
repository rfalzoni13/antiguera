using Antiguera.Administrador.Client.Base;
using Antiguera.Administrador.Client.Interface;
using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Client
{
    public class UsuarioClient : ClientBase<UsuarioModel>, IUsuarioClient
    {
        public async Task<UsuarioModel> ListarPorIdentityId(string userId, string token)
        {
            string url = $"{UrlConfiguration.UsuarioGetByUserId}?UserId={userId}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<UsuarioModel>();
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