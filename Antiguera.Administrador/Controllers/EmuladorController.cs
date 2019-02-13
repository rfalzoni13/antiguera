using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class EmuladorController : BaseController
    {
        // GET: Emulador
        public ActionResult Index(int pagina = 1)
        {
            try
            {
                if(HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
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
                    if(!string.IsNullOrEmpty(token))
                    {
                        var lista = ListarEmuladores(token);
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
                        if(!string.IsNullOrEmpty(token))
                        {
                            var lista = ListarEmuladores(token);
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
            catch(Exception ex)
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
                if(HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                { 
                    return View();
                }
                else
                {
                    Session["ErroMensagem"] = "Acesso restrito!";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch(Exception ex)
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
        public ActionResult Cadastrar(EmuladorModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {
                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                        {
                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                            model.FileEmulador.SaveAs(emuPath);
                            model.UrlArquivo = "/Content/Consoles/Emuladores/" + emuladorFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (CadastrarEmulador(model, token))
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
                                        if (CadastrarEmulador(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                                                {
                                                    var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                                                    var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                                                    System.IO.File.Delete(emuPath);
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
                                if (CadastrarEmulador(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                                        {
                                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                                            System.IO.File.Delete(emuPath);
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
                        var model = BuscarEmuladorPorId(id, token);
                        if (TempData["Unauthorized"] != null)
                        {
                            token = PegarTokenRefreshAtual();
                            if (!string.IsNullOrEmpty(token))
                            {
                                model = BuscarEmuladorPorId(id, token);
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
                            var model = BuscarEmuladorPorId(id, token);
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
        public ActionResult Editar(EmuladorModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {
                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                        {
                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                            model.FileEmulador.SaveAs(emuPath);
                            model.UrlArquivo = "/Content/Consoles/Emuladores/" + emuladorFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (AtualizarEmulador(model, token))
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
                                        if (AtualizarEmulador(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                                                {
                                                    var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                                                    var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                                                    System.IO.File.Delete(emuPath);
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
                                if (AtualizarEmulador(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                                        {
                                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                                            System.IO.File.Delete(emuPath);
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
                            var model = BuscarEmuladorPorId(id, token);

                            if (TempData["Unauthorized"] != null)
                            {
                                if (model != null)
                                {
                                    ExcluirEmulador(model, token);
                                }
                            }
                            else
                            {
                                token = PegarTokenRefreshAtual();
                                if (!string.IsNullOrEmpty(token))
                                {
                                    model = BuscarEmuladorPorId(id, token);
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        if (model != null)
                                        {
                                            ExcluirEmulador(model, token);
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
                                var model = BuscarEmuladorPorId(id, token);
                                if (TempData["Unauthorized"] != null)
                                {
                                    if (model != null)
                                    {
                                        ExcluirEmulador(model, token);
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