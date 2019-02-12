using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using Antiguera.Infra.Cross.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.Usuarios = ListarUsuarios();
            model.Jogos = ListarJogos();
            return View(model);
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel();

            if (HttpContext.Request.Cookies.AllKeys.Contains("Antiguera"))
            {
                if (HttpContext.Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var token = PegarTokenAtual();
                    var refreshToken = PegarTokenRefreshAtual();

                    if(!string.IsNullOrEmpty(token))
                    {
                        var user = HttpContext.GetOwinContext().Authentication.User.Identity.Name;

                        Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + user).Result)
                        {
                            if (responseUsuario.IsSuccessStatusCode)
                            {
                                var resultUsuario = responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;
                            }
                            else if(!string.IsNullOrEmpty(refreshToken))
                            {
                                var newToken = RefreshToken(refreshToken);
                                if(newToken != null)
                                {
                                    Cliente.DefaultRequestHeaders.Clear();
                                    Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + newToken);

                                    using (var newResponseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosUsuarios + user).Result)
                                    {
                                        if(newResponseUsuario.IsSuccessStatusCode)
                                        {
                                            var resultUsuario = responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.ErroMensagem = "Sua sessão expirou, faça login novamente!";
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return View(model);
                                }
                            }
                            else
                            {
                                ViewBag.ErroMensagem = "Sua sessão expirou, faça login novamente!";
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErroMensagem = "Sua sessão expirou, faça login novamente!";
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErroMensagem = "Sua sessão expirou, faça login novamente!";
                    HttpContext.GetOwinContext().Authentication.SignOut();
                    return View(model);
                }
            }
            else
            {
                HttpContext.GetOwinContext().Authentication.SignOut();

                if (Session["logout"] != null)
                {
                    ViewBag.StatusMensagem = Session["logout"];
                    Session.Clear();
                }

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var responseFirstLogin = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlLoginAdmin, model).Result)
                {
                    if (responseFirstLogin.IsSuccessStatusCode)
                    {
                        var token = ObterToken(model);

                        if (token != null)
                        {
                            var user = responseFirstLogin.Content.ReadAsAsync<ApplicationUser>().Result;

                            var applicationSign = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

                            if (model.RememberMe)
                            {
                                AuthenticationProperties options = new AuthenticationProperties()
                                {
                                    AllowRefresh = true,
                                    IsPersistent = true,
                                    ExpiresUtc = DateTime.Now.AddSeconds(token.expires_in)
                                };

                                var claims = new[]
                                {
                                    new Claim(ClaimTypes.Name, user.UserName),
                                    new Claim("AccessToken", token.access_token),
                                    new Claim("RefreshToken", token.refresh_token)
                                };

                                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                                HttpContext.Request.GetOwinContext().Authentication.SignIn(options, identity);
                                applicationSign.SignIn(user, true, false);
                            }
                            else
                            {
                                Session["token"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(token.access_token));
                                applicationSign.SignIn(user, false, false);
                            }

                            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);

                            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + model.UserName).Result)
                            {
                                if (responseUsuario.IsSuccessStatusCode)
                                {
                                    var resultUsuario = responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;
                                }
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErroMensagem = responseFirstLogin.Content.ReadAsAsync<string>().Result;
                        return View(model);
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Preencha todos os campos corretamente!";
                return View(model);
            }
        }

        public ActionResult Logoff()
        {
            if (Session != null)
            {
                Session.Clear();
            }

            HttpContext.GetOwinContext().Authentication.SignOut();

            Session["logout"] = "Você foi desconectado!";

            return RedirectToAction("Login");
        }
    }
}