using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                var model = new LoginModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                return View("Error");
            }
        }

        // POST: Account
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                var url = UrlConfiguration.Login;

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", model.Login),
                        new KeyValuePair<string, string>("password", model.Password)
                    });

                    HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        string resultContent = response.Content.ReadAsStringAsync().Result;

                        var token = JsonConvert.DeserializeObject<TokenModel>(resultContent);

                        AuthenticationProperties options = new AuthenticationProperties();

                        options.AllowRefresh = true;
                        options.IsPersistent = true;
                        options.ExpiresUtc = DateTime.UtcNow.AddSeconds(DateTime.Now.Second + token.Expire.Value.Second);

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, model.Login),
                            new Claim("AcessToken", string.Format("Bearer {0}", token.AccessToken)),
                        };

                        var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                        Request.GetOwinContext().Authentication.SignIn(options, identity);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Login e/ou Senha incorretos!");
                        return View();
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                return View("Error");
            }
        }

        //POST: Logout
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.VerifyCode;


                    HttpResponseMessage response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // GET: /Account/VerificarCodigo
        [AllowAnonymous]
        public async Task<ActionResult> VerificarCodigo(string provider, string returnUrl, bool rememberMe)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.VerifyCode;


                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var userFactors = response.Content.ReadAsAsync<IList<string>>().Result;
                        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

                        return View(new VerifyCodeModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // POST: /Account/VerificarCodigo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerificarCodigo(VerifyCodeModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.VerifyCode;


                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<ReturnCodeStatusModel>();
                        switch (result.Status)
                        {
                            case ESignInStatusCode.Success:
                                return RedirectToLocal(model.ReturnUrl);
                            case ESignInStatusCode.LockedOut:
                                return View("Lockout");
                            case ESignInStatusCode.Failure:
                            default:
                                ModelState.AddModelError(string.Empty, "Código inválido.");
                                return View(model);
                        }
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // GET: /Account/Registrar
        [HttpGet]
        public ActionResult Registrar()
        {
            try
            {
                var model = new RegistrarModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                return View("Error");
            }
        }

        // POST: /Account/Registrar
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registrar(RegistrarModel model)
        {
            _logger.Debug("Iniciando");
            try
            {
                if (!model.AcceptTerms)
                {
                    _logger.Error("Você deve aceitar os termos de uso!");
                    ModelState.AddModelError(string.Empty, "Você deve aceitar os termos de uso!");
                }

                if (ModelState.IsValid)
                {

                    using (HttpClient client = new HttpClient())
                    {
                        var url = UrlConfiguration.Register;


                        HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Account", "Login");
                            // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
                            // Enviar um email com este link
                            // string code = await UserManager().GenerateEmailConfirmationTokenAsync(user.Id);
                            // var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            // await UserManager().SendEmailAsync(user.Id, "Confirmar sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");

                        }
                        else
                        {
                            StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                            throw new Exception(statusCode.Message);
                        }
                    }
                }

                // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // GET: /Account/ConfirmarEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmarEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }

                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.ConfirmEmail;

                    var model = new ConfirmEmailCodeModel
                    {
                        UserId = userId,
                        Code = code
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<IdentityResultCodeModel>();

                        return View(result.Succeeded ? "ConfirmarEmail" : "Error");
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        //GET: /Account/EsqueciMinhaSenha
        public ActionResult EsqueciMinhaSenha()
        {
            var model = new ForgotPasswordModel();

            return View(model);
        }

        //
        // POST: /Account/EsqueciMinhaSenha
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EsqueciMinhaSenha(ForgotPasswordModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.ForgotPassword;

                    model.CallBackUrl = Url.Action("RecuperarSenha", "Account", new { userId = "{0}", code = "{1}" }, protocol: Request.Url.Scheme);

                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        //
        // GET: /Account/ConfirmacaoRecuperacaoSenha
        [AllowAnonymous]
        public ActionResult ConfirmacaoRecuperacaoSenha()
        {
            return View();
        }

        //
        // GET: /Account/RecuperarSenha
        [AllowAnonymous]
        public ActionResult RecuperarSenha(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/RecuperarSenha
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecuperarSenha(ResetPasswordModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.ResetPassword;

                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<IdentityResultCodeModel>();

                        if (result.Succeeded)
                        {
                            return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
                        }

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        //
        // POST: /Account/LoginExterno
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginExterno(string provider, string returnUrl)
        {
            // Solicitar um redirecionamento para o provedor de logon externo
            return new ChallengeResultHelper(provider, Url.Action("CallbackLoginExterno", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/EnviarCodigo
        [AllowAnonymous]
        public async Task<ActionResult> EnviarCodigo(string returnUrl, bool rememberMe)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.GetSmsProviders;


                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var userFactors = response.Content.ReadAsAsync<IList<string>>().Result;
                        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

                        return View(new SendCodeModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // POST: /Account/EnviarCodigo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnviarCodigo(SendCodeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.SendCode;


                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("VerificarCodigo", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}