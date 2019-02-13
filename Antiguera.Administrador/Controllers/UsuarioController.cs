using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                    if (TempData["Mensagem"] != null)
                    {
                        ViewBag.Mensagem = TempData["Mensagem"];
                    }

                    if (TempData["ErroMensagem"] != null)
                    {
                        ViewBag.ErroMensagem = TempData["ErroMensagem"];
                    }

                    var token = PegarTokenAtual();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var lista = ListarUsuarios(token);
                        if (TempData["Unauthorized"] != null)
                        {
                            Session["ErroMensagem"] = ViewBag.ErroMensagem;
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            lista.OrderBy(j => j.Id).ToPagedList(pagina, 4);
                        }
                    }
                    else
                    {
                        token = PegarTokenRefreshAtual();
                        if (!string.IsNullOrEmpty(token))
                        {
                            var lista = ListarUsuarios(token);
                            if (TempData["Unauthorized"] != null)
                            {
                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login");
                            }
                            else
                            {
                                lista.OrderBy(j => j.Id).ToPagedList(pagina, 4);
                            }
                        }
                    }
                    return View();
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

                    var token = PegarTokenAtual();
                    if(!string.IsNullOrEmpty(token))
                    {
                        var acessos = ListarAcessos(token);
                        if (TempData["Unauthorized"] != null)
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                acessos = ListarAcessos(token);
                                if (TempData["Unauthorized"] != null)
                                {
                                    Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return RedirectToAction("Login", "Home");
                                }
                            }
                            else
                            {
                                Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login", "Home");
                            }
                        }

                        foreach(var acesso in acessos)
                        {
                            if(acesso != null)
                            {
                                model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                            }
                        }
                    }
                    else
                    {
                        token = PegarTokenRefreshAtual();
                        if(!string.IsNullOrEmpty(token))
                        {
                            var acessos = ListarAcessos(token);
                            if(TempData["Unauthorized"] != null)
                            {
                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login");
                            }

                            foreach (var acesso in acessos)
                            {
                                if (acesso != null)
                                {
                                    model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                                }
                            }
                        }
                        else
                        {
                            Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
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
                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                        {
                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                            model.FileFoto.SaveAs(photoPath);
                            model.UrlFotoUpload = "/Content/Images/Profile/" + fotoFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (CadastrarUsuario(model, token))
                            {
                                return View("Index");
                            }
                            else
                            {
                                if (TempData["Unauthorized"] != null)
                                {
                                    token = PegarTokenRefreshAtual();
                                    if (!string.IsNullOrEmpty(token))
                                    {
                                        if (CadastrarUsuario(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                                                {
                                                    var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                                                    var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                                                    System.IO.File.Delete(photoPath);
                                                }

                                                return RedirectToAction("Login", "Home");
                                            }
                                            return View(model);
                                        }
                                    }
                                    Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return RedirectToAction("Login", "Home");
                                }
                                return View(model);
                            }
                        }
                        else
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                if (CadastrarUsuario(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                                        {
                                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                                            System.IO.File.Delete(photoPath);
                                        }

                                        return RedirectToAction("Login", "Home");
                                    }
                                }
                            }
                            Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
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

        // GET: Editar
        public ActionResult Editar(int id)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var token = PegarTokenAtual();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var model = BuscarUsuarioPorId(id, token);
                        if (TempData["Unauthorized"] != null)
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                model = BuscarUsuarioPorId(id, token);
                                if (TempData["Unauthorized"] != null)
                                {
                                    Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return RedirectToAction("Login", "Home");
                                }
                                else
                                {
                                    if (model != null)
                                    {
                                        return View(model);
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                            else
                            {
                                Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            if (model != null)
                            {
                                return View(model);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else
                    {
                        token = PegarTokenRefreshAtual();
                        if (!string.IsNullOrEmpty(token))
                        {
                            var model = BuscarUsuarioPorId(id, token);
                            if (TempData["Unauthorized"] != null)
                            {
                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login", "Home");
                            }
                            else
                            {
                                if (model != null)
                                {
                                    return View(model);
                                }
                                else
                                {
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                        else
                        {
                            Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }
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
                    if (model != null && ModelState.IsValid)
                    {
                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                        {
                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                            model.FileFoto.SaveAs(photoPath);
                            model.UrlFotoUpload = "/Content/Images/Profile/" + fotoFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (AtualizarUsuario(model, token))
                            {
                                return View("Index");
                            }
                            else
                            {
                                if (TempData["Unauthorized"] != null)
                                {
                                    token = PegarTokenRefreshAtual();
                                    if (!string.IsNullOrEmpty(token))
                                    {
                                        if (AtualizarUsuario(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                                                {
                                                    var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                                                    var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                                                    System.IO.File.Delete(photoPath);
                                                }

                                                return RedirectToAction("Login", "Home");
                                            }
                                            return View(model);
                                        }
                                    }
                                    Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return RedirectToAction("Login", "Home");
                                }
                                return View(model);
                            }
                        }
                        else
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                if (AtualizarUsuario(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                                        {
                                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                                            System.IO.File.Delete(photoPath);
                                        }

                                        return RedirectToAction("Login", "Home");
                                    }
                                }
                            }
                            Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
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
                        var token = PegarTokenAtual();
                        if (!string.IsNullOrEmpty(token))
                        {
                            var model = BuscarUsuarioPorId(id, token);

                            if (TempData["Unauthorized"] != null)
                            {
                                if (model != null)
                                {
                                    ExcluirUsuario(model, token);
                                }
                            }
                            else
                            {
                                token = PegarTokenRefreshAtual();
                                if (!string.IsNullOrEmpty(token))
                                {
                                    model = BuscarUsuarioPorId(id, token);
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        if (model != null)
                                        {
                                            ExcluirUsuario(model, token);
                                        }
                                    }
                                    else
                                    {
                                        HttpContext.GetOwinContext().Authentication.SignOut();
                                        return RedirectToAction("Login", "Home");
                                    }
                                }
                                Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                var model = BuscarUsuarioPorId(id, token);
                                if (TempData["Unauthorized"] != null)
                                {
                                    if (model != null)
                                    {
                                        ExcluirUsuario(model, token);
                                    }
                                }
                                else
                                {
                                    HttpContext.GetOwinContext().Authentication.SignOut();
                                    return RedirectToAction("Login", "Home");
                                }
                            }
                            Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
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