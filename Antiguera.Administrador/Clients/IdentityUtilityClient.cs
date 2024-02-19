using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Common;
using Antiguera.Administrador.Models.Identity;
using Antiguera.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Antiguera.Administrador.Clients
{
    public class IdentityUtilityClient
    {
        #region DOIS FATORES
        public async Task<ICollection<string>> ObterAutenticacaoDoisFatores()
        {
            var url = UrlConfigurationHelper.IdentityUtilityGetTwoFactorProviders;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ICollection<string>>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task EnviarCodigoDoisFatores(SendCodeModel model)
        {
            var url = UrlConfigurationHelper.IdentityUtilitySendTwoFactorProviderCode;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (!response.IsSuccessStatusCode)
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<ReturnVerifyCodeModel> VerificarCodigoDoisFatores(VerifyCodeModel model)
        {
            string url = UrlConfigurationHelper.IdentityUtilityVerifyCodeTwoFactor;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ReturnVerifyCodeModel>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }
        #endregion

        #region CONFIRMAÇÃO DE EMAIL E TELEFONE
        public async Task<string> EnviarCodigoConfirmacaoEmail(GenerateTokenEmailModel model)
        {
            string url = UrlConfigurationHelper.IdentityUtilitySendEmailConfirmationCode;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<string> EnviarCodigoConfirmacaoTelefone(GenerateTokenPhoneModel model)
        {
            string url = UrlConfigurationHelper.IdentityUtilitySendPhoneConfirmationCode;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<string> VerificarCodigoConfirmacaoEmail(ConfirmEmailCodeModel model)
        {
            string url = UrlConfigurationHelper.IdentityUtilityVerifyEmailConfirmationCode;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<string> VerificarCodigoConfirmacaoTelefone(ConfirmPhoneCodeModel model)
        {
            string url = UrlConfigurationHelper.IdentityUtilityVerifyPhoneConfirmationCode;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }
        #endregion
    }
}