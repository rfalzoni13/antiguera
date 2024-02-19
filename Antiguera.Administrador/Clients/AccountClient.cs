using Antiguera.Administrador.Models.Identity;
using Antiguera.Administrador.Models.Common;
using Antiguera.Utils.Helpers;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.Administrador.Clients
{
    public class AccountClient
    {
        #region LOGIN
        public async Task Login(LoginModel model, HttpRequestBase request)
        {
            var url = UrlConfigurationHelper.AccountLogin;

            using (HttpClient httpClient = new HttpClient())
            {
                //Setar Timeout de conexão
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                HttpContent content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", model.Login),
                        new KeyValuePair<string, string>("password", model.Password)
                });

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string resultContent = response.Content.ReadAsStringAsync().Result;

                    var token = JsonConvert.DeserializeObject<TokenModel>(resultContent);

                    AuthenticationProperties options = new AuthenticationProperties();

                    options.AllowRefresh = true;
                    options.IsPersistent = true;
                    options.ExpiresUtc = token.Expire;

                    var claims = new[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, token.UserId),
                            new Claim(ClaimTypes.Role, token.RoleId),
                            new Claim(ClaimTypes.Expiration, token.Expire.ToString()),
                            new Claim("AccessToken", token.AccessToken),
                            new Claim("RefreshToken",token.RefreshToken),
                        };

                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                    request.GetOwinContext().Authentication.SignIn(options, identity);
                }
                else
                {
                    var errorResponse = response.Content.ReadAsAsync<ResponseErrorLogin>().Result;

                    throw new ApplicationException(errorResponse.error_description);
                }
            }
        }

        public async Task RefreshToken()
        {
            var url = UrlConfigurationHelper.AccountLogin;

            string refreshToken = HttpContext.Current.GetOwinContext().Authentication.User.Claims
                .FirstOrDefault(x => x.Type.Contains("RefreshToken")).Value 
                ?? throw new Exception("Token expirado ou inválido");

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", refreshToken)
                    });

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string resultContent = await response.Content.ReadAsStringAsync();

                    HttpContext.Current.GetOwinContext().Authentication.SignOut("ApplicationCookie");

                    var token = JsonConvert.DeserializeObject<TokenModel>(resultContent);

                    AuthenticationProperties options = new AuthenticationProperties();

                    options.AllowRefresh = true;
                    options.IsPersistent = true;
                    options.ExpiresUtc = token.Expire;

                    var claims = new[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, token.UserId),
                            new Claim(ClaimTypes.Role, token.RoleId),
                            new Claim(ClaimTypes.Expiration, token.Expire.ToString()),
                            new Claim("AccessToken", token.AccessToken),
                            new Claim("RefreshToken",token.RefreshToken)
                        };

                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                    HttpContext.Current.GetOwinContext().Authentication.SignIn(options, identity);
                }
                else
                {
                    throw new ApplicationException("Login e/ou Senha incorretos!");
                }
            }
        }

        public async Task Logout(HttpRequestBase request)
        {
            var url = UrlConfigurationHelper.AccountLogout;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }
        #endregion

        #region LOGIN EXTERNO
        public async Task LoginExterno(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public async Task<IList<ExternalLoginModel>> ObterLoginsExternos()
        {
            string url = UrlConfigurationHelper.AccountGetExternalLogins;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IList<ExternalLoginModel>>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<IList<IdentityResultCodeModel>> AdicionarLoginExterno(AddExternalLoginBindingModel model)
        {
            string url = UrlConfigurationHelper.AccountAddExternalLogin;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IList<IdentityResultCodeModel>>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<IList<IdentityResultCodeModel>> AdicionarUsuarioLoginExterno(RegisterExternalBindingModel model)
        {
            string url = UrlConfigurationHelper.AccountAddUserExternalLogin;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IList<IdentityResultCodeModel>>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }

        public async Task<IList<IdentityResultCodeModel>> RemoverLoginExterno(RegisterExternalBindingModel model)
        {
            string url = UrlConfigurationHelper.AccountAddUserExternalLogin;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IList<IdentityResultCodeModel>>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new Exception(statusCode.Message);
                }
            }
        }
        #endregion

        #region SENHA
        public async Task<IdentityResultCodeModel> AlterarSenha(ChangePasswordModel model)
        {
            var url = UrlConfigurationHelper.AccountChangePassword;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IdentityResultCodeModel>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public async Task<string> EsqueciMinhaSenha(ForgotPasswordModel model)
        {
            var url = UrlConfigurationHelper.AccountForgotPassword;

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

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public async Task<IdentityResultCodeModel> RecuperarSenha(ResetPasswordModel model)
        {
            var url = UrlConfigurationHelper.AccountResetPassword;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IdentityResultCodeModel>();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        #endregion
    }
}