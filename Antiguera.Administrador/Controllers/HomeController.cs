using Antiguera.Administrador.Config;
using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var model = new HomeModel();
                    model.Usuarios = ListarUsuarios();
                    model.Jogos = ListarJogos();
                    model.Programas = ListarProgramas();

                    model.InfMaquina = GetInformacoes();

                    if (Session["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login");
                    }
                    return View(model);
                }
                else
                {
                    Session["Unauthorized"] = "Sua sessão expirou, faça login novamente!";
                    return RedirectToAction("Login");
                }
            }

            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel();

            try
            {
                if (HttpContext.Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();

                    if (Session["logout"] != null)
                    {
                        ViewBag.StatusMensagem = Session["logout"];
                        Session.Clear();
                    }
                    if (Session["ErroMensagem"] != null)
                    {
                        ViewBag.ErroMensagem = Session["ErroMensagem"];
                        Session.Clear();
                    }

                    if (Session["Unauthorized"] != null)
                    {
                        ViewBag.ErroMensagem = Session["Unauthorized"];
                        Session.Clear();
                    }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErroMensagem = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
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

                                var claims = new[]
                                {
                                    new Claim(ClaimTypes.Name, user.UserName),
                                    new Claim("AccessToken", token.access_token),
                                    new Claim("RefreshToken", token.refresh_token)
                                };

                                if (model.RememberMe)
                                {
                                    AuthenticationProperties options = new AuthenticationProperties()
                                    {
                                        AllowRefresh = true,
                                        IsPersistent = true,
                                        ExpiresUtc = DateTime.Now.AddSeconds(token.expires_in)
                                    };

                                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                                    HttpContext.Request.GetOwinContext().Authentication.SignIn(options, identity);
                                }
                                else
                                {
                                    AuthenticationProperties options = new AuthenticationProperties()
                                    {
                                        AllowRefresh = false,
                                        IsPersistent = false,
                                    };

                                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                                    HttpContext.Request.GetOwinContext().Authentication.SignIn(options, identity);
                                }
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var result = responseFirstLogin.Content.ReadAsAsync<StatusCode>().Result;
                            ViewBag.ErroMensagem = result.Message;
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
            catch (Exception ex)
            {
                ViewBag.ErroMensagem = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return View(model);
            }
        }
        
        public ActionResult RecuperarSenha()
        {
            if (HttpContext.Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            if (Session["ErroMensagem"] != null)
            {
                ViewBag.ErroMensagem = Session["ErroMensagem"];
                Session.Clear();
            }

            if (Session["Unauthorized"] != null)
            {
                ViewBag.ErroMensagem = Session["Unauthorized"];
                Session.Clear();
            }

            var model = new PasswordRecoveryModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult RecuperarSenha(PasswordRecoveryModel model)
        {
            if(ModelState.IsValid)
            {

            }
        }

        public ActionResult Logoff()
        {
            try
            {
                if (Session != null)
                {
                    Session.Clear();
                }

                HttpContext.GetOwinContext().Authentication.SignOut();

                Session["logout"] = "Você foi desconectado!";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login");
            }
        }
    }
}