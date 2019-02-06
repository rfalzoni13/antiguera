﻿using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
                var content = new List<KeyValuePair<string, string>>(new[]
                {
                    new KeyValuePair<string, string>("username", model.Email),
                    new KeyValuePair<string, string>("password", model.Senha),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
                var stringContent = new FormUrlEncodedContent(content);

                using (var responseLogin = Cliente.PostAsync(url.UrlApi + url.UrlLogin, stringContent).Result)
                {
                    if (responseLogin.IsSuccessStatusCode)
                    {
                        var resultLogin = responseLogin.Content.ReadAsAsync<ResponseLoginModel>().Result;

                        Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + resultLogin.access_token);

                        using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + model.Email).Result)
                        {
                            if (responseUsuario.IsSuccessStatusCode)
                            {
                                var resultUsuario = responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

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
                        return RedirectToAction("Index", "Home");
                    }
                    else if (responseLogin.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = responseLogin.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return View(model);
                    }

                    else
                    {
                        var result = responseLogin.Content.ReadAsAsync<ResponseErrorLogin>().Result;
                        ViewBag.ErroMensagem = result.error_description;
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
            if (Response.Cookies.Get("usrDt") != null && Response.Cookies.Get("tknUs") != null)
            {
                ClearCookies();
            }
            else if (Session != null)
            {
                Session.Abandon();
            }

            TempData["logout"] = "Você foi desconectado!";

            return RedirectToAction("Login", "Home");
        }
    }
}