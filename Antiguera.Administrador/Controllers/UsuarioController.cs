using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        // GET: Usuario
        public ActionResult Index(int pagina = 1)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (Session["Mensagem"] != null)
                    {
                        ViewBag.Mensagem = Session["Mensagem"];
                        Session.Clear();
                    }

                    if (Session["ErroMensagem"] != null)
                    {
                        ViewBag.ErroMensagem = Session["ErroMensagem"];
                        Session.Clear();
                    }

                    var lista = ListarUsuarios();
                    if (Session["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }
                    return View(lista.OrderBy(x => x.Id).ToPagedList(pagina, 4));
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Cadastrar
        public ActionResult Cadastrar()
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var model = new UsuarioModel();
                    model.ListaAcessos = new List<SelectListItem>();

                    model.ListaAcessos.Add(new SelectListItem() { Text = "Selecione uma opção...", Value = "0" });

                    var acessos = ListarAcessos();
                    if (TempData["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }

                    foreach(var acesso in acessos)
                    {
                        if(acesso != null)
                        {
                            model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                        }
                    }
                    
                    return View(model);
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(UsuarioModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {

                        CadastrarUsuario(model);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Detalhes
        public ActionResult Detalhes(int id)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var model = BuscarUsuarioPorId(id);

                    model.Acesso = BuscarAcessoPorId(model.AcessoId);

                    if (TempData["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }

                    if (model != null)
                    {
                        if (model.Novo == true)
                        {
                            AtualizarUsuarioNovo(model);
                        }

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var model = BuscarUsuarioPorId(id);
                    model.ListaAcessos = new List<SelectListItem>();

                    model.ListaAcessos.Add(new SelectListItem() { Text = "Selecione uma opção...", Value = "0" });

                    var acessos = ListarAcessos();
                    if (TempData["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }

                    if (model != null)
                    {
                        if (model.Novo == true)
                        {
                            AtualizarUsuarioNovo(model);
                        }

                        foreach (var acesso in acessos)
                        {
                            if (acesso != null)
                            {
                                model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                            }
                        }

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Editar
        [HttpPost]
        public ActionResult Editar(UsuarioModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var oldModel = BuscarUsuarioPorId(model.Id);
                    if (oldModel != null)
                    {
                        model.Senha = oldModel.Senha;
                        model.Created = oldModel.Created;
                        model.UrlFotoUpload = oldModel.UrlFotoUpload;
                    }

                    ModelState.Remove("Senha");
                    ModelState.Remove("ConfirmarSenha");

                    if (model != null && ModelState.IsValid)
                    {
                        AtualizarUsuario(model);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Excluir
        public ActionResult Excluir(int id)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (id == 0)
                    {
                        ViewBag.ErroMensagem = "Parâmetros incorretos!";
                    }
                    else
                    {
                        var model = BuscarUsuarioPorId(id);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        if(model != null)
                        {
                            ExcluirUsuario(model);
                        }                        

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                Session["ErroMensagem"] = "Erro: " + ex.Message;
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
                return RedirectToAction("Login", "Home");
            }
        }
    }
}