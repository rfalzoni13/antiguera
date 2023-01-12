using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.Administrador.Client
{
    public class AccountClient
    {
        #region LOGIN
        public void Login(LoginModel model, HttpRequestBase request)
        {
            var url = UrlConfiguration.AccountLogin;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", model.Login),
                        new KeyValuePair<string, string>("password", model.Password)
                    });

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
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
                            new Claim("AccessToken", string.Format("Bearer {0}", token.AccessToken)),
                        };

                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                    request.GetOwinContext().Authentication.SignIn(options, identity);
                }
                else
                {
                    throw new ApplicationException("Login e/ou Senha incorretos!");
                }
            }
        }

        public async Task Logout(HttpRequestBase request)
        {
            var url = UrlConfiguration.AccountLogout;

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
            string url = UrlConfiguration.AccountGetExternalLogins;

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
            string url = UrlConfiguration.AccountAddExternalLogin;

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
            string url = UrlConfiguration.AccountAddUserExternalLogin;

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
            string url = UrlConfiguration.AccountAddUserExternalLogin;

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

        #region DOIS FATORES
        public async Task<ICollection<string>> ObterAutenticacaoDoisFatores()
        {
            var url = UrlConfiguration.AccountGetTwoFactorProviders;

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
            var url = UrlConfiguration.AccountSendTwoFactorProviderCode;

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
            string url = UrlConfiguration.AccountVerifyCodeTwoFactor;

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

        #region CADASTRO
        public async Task<string> Registrar(RegistrarModel model)
        {
            var url = UrlConfiguration.AccountRegister;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                if (response.IsSuccessStatusCode)
                {
                    // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar um email com este link
                    // string code = await UserManager().GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager().SendEmailAsync(user.Id, "Confirmar sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");

                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    StatusCodeModel statusCode = await response.Content.ReadAsAsync<StatusCodeModel>();

                    throw new ApplicationException(statusCode.Message);
                }
            }
        }

        public async Task<string> Atualizar(RegistrarModel model)
        {
            var url = UrlConfiguration.AccountUpdate;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url, model);
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

        public async Task<string> Apagar(RegistrarModel model)
        {
            var url = UrlConfiguration.AccountDelete;

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
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

        #region CONFIRMAÇÃO DE EMAIL E TELEFONE
        public async Task<string> EnviarCodigoConfirmacaoEmail(GenerateTokenEmailModel model)
        {
            string url = UrlConfiguration.AccountSendEmailConfirmationCode;

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
            string url = UrlConfiguration.AccountSendPhoneConfirmationCode;

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
            string url = UrlConfiguration.AccountVerifyEmailConfirmationCode;

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
            string url = UrlConfiguration.AccountVerifyPhoneConfirmationCode;

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

        #region SENHA
        public async Task<IdentityResultCodeModel> AlterarSenha(ChangePasswordModel model)
        {
            var url = UrlConfiguration.AccountChangePassword;

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
            var url = UrlConfiguration.AccountForgotPassword;

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
            var url = UrlConfiguration.AccountResetPassword;

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