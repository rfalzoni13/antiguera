using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using Antiguera.Infra.Cross.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            if (TempData["logout"] != null)
            {
                ViewBag.StatusMensagem = TempData["logout"];
                Session.Clear();
            }
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var responseFirstLogin =  Cliente.PostAsJsonAsync(url.UrlApi + url.UrlLoginAdmin, model).Result)
                {
                    if (responseFirstLogin.IsSuccessStatusCode)
                    {
                        var content = new List<KeyValuePair<string, string>>(new[]
                        {
                            new KeyValuePair<string, string>("username", model.UserName),
                            new KeyValuePair<string, string>("password", model.Password),
                            new KeyValuePair<string, string>("grant_type", "password")
                        });

                        var stringContent = new FormUrlEncodedContent(content);

                        using (var responseSecondLogin = Cliente.PostAsync(url.UrlApi + url.UrlToken, stringContent).Result)
                        {
                            if (responseSecondLogin.IsSuccessStatusCode)
                            {
                                var resultLogin = responseSecondLogin.Content.ReadAsAsync<ResponseLoginModel>().Result;

                                Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + resultLogin.access_token);

                                using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + model.UserName).Result)
                                {
                                    if (responseUsuario.IsSuccessStatusCode)
                                    {
                                        var resultUsuario = responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

                                        var user = responseFirstLogin.Content.ReadAsAsync<ApplicationUser>().Result;

                                        var applicationSign = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                                                                                
                                        applicationSign.SignIn(user, true, true);

                                        if (model.RememberMe)
                                        {
                                            SetInfCookies(resultLogin, resultUsuario);
                                        }
                                        else
                                        {
                                            Session["token"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(resultLogin.access_token));
                                            Session["userData"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(resultUsuario.Id + resultUsuario.Email));
                                        }
                                    }
                                }

                                return RedirectToAction("Index");
                            }
                            else if (responseSecondLogin.StatusCode == HttpStatusCode.InternalServerError)
                            {
                                var result = responseSecondLogin.Content.ReadAsAsync<StatusCode>().Result;
                                ViewBag.ErroMensagem = result.Mensagem;
                                return View(model);
                            }

                            else
                            {
                                var result = responseSecondLogin.Content.ReadAsAsync<ResponseErrorLogin>().Result;
                                ViewBag.ErroMensagem = result.error_description;
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Mensagem = responseFirstLogin.Content.ReadAsAsync<string>().Result;
                        return View(model);
                    }
                }
            }
            else
            {
                ViewBag.Mensagem = "Preencha todos os campos corretamente!";
                return View(model);
            }
        }

        public ActionResult Logoff()
        {
            if(Request.Cookies["usrDt"] != null && Request.Cookies["tknUs"] != null)
            {
                ClearCookies();
            }

            if (Session != null)
            {
                Session.Abandon();
            }

            HttpContext.GetOwinContext().Authentication.SignOut();

            TempData["logout"] = "Você foi desconectado!";

            return RedirectToAction("Login");
        }
    }
}