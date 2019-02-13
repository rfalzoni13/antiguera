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
    public class RomController : BaseController
    {
        // GET: Rom
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
                        var lista = ListarRoms(token);
                        if (TempData["Unauthorized"] != null)
                        {
                            Session["ErroMensagem"] = ViewBag.ErroMensagem;
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
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
                            var lista = ListarRoms(token);
                            if (TempData["Unauthorized"] != null)
                            {
                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                HttpContext.GetOwinContext().Authentication.SignOut();
                                return RedirectToAction("Login", "Home");
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

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(RomModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {
                        if (model.FileRom != null && model.FileRom.ContentLength > 0)
                        {
                            var romFileName = Path.GetFileName(model.FileRom.FileName);
                            var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                            model.FileRom.SaveAs(romPath);
                            model.UrlArquivo = "/Content/Consoles/Roms/" + romFileName;
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                            model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (CadastrarRom(model, token))
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
                                        if (CadastrarRom(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileRom != null && model.FileRom.ContentLength > 0)
                                                {
                                                    var romFileName = Path.GetFileName(model.FileRom.FileName);
                                                    var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                                                    System.IO.File.Delete(romPath);
                                                }
                                                
                                                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                                                {
                                                    var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                                                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                                                    System.IO.File.Delete(boxPath);
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
                                if (CadastrarRom(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        var romFileName = Path.GetFileName(model.FileRom.FileName);
                                        var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                                        System.IO.File.Delete(romPath);

                                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                                        System.IO.File.Delete(boxPath);

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
                    if(!string.IsNullOrEmpty(token))
                    {
                        var model = BuscarRomPorId(id, token);
                        if (TempData["Unauthorized"] != null)
                        {
                            token = PegarTokenRefreshAtual();
                            if(!string.IsNullOrEmpty(token))
                            {
                                model = BuscarRomPorId(id, token);
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
                            var model = BuscarRomPorId(id, token);
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
        public ActionResult Editar(RomModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {
                        if (model.FileRom != null && model.FileRom.ContentLength > 0)
                        {
                            var romFileName = Path.GetFileName(model.FileRom.FileName);
                            var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                            model.FileRom.SaveAs(romPath);
                            model.UrlArquivo = "/Content/Consoles/Roms/" + romFileName;
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                            model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                        }

                        var token = PegarTokenAtual();

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (AtualizarRom(model, token))
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
                                        if (AtualizarRom(model, token))
                                        {
                                            return View("Index");
                                        }
                                        else
                                        {
                                            if (TempData["Unauthorized"] != null)
                                            {
                                                Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                                HttpContext.GetOwinContext().Authentication.SignOut();

                                                if (model.FileRom != null && model.FileRom.ContentLength > 0)
                                                {
                                                    var romFileName = Path.GetFileName(model.FileRom.FileName);
                                                    var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                                                    System.IO.File.Delete(romPath);
                                                }

                                                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                                                {
                                                    var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                                                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                                                    System.IO.File.Delete(boxPath);
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
                                if (AtualizarRom(model, token))
                                {
                                    return View("Index");
                                }
                                else
                                {
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        Session["ErroMensagem"] = ViewBag.ErroMensagem;
                                        HttpContext.GetOwinContext().Authentication.SignOut();

                                        if (model.FileRom != null && model.FileRom.ContentLength > 0)
                                        {
                                            var romFileName = Path.GetFileName(model.FileRom.FileName);
                                            var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                                            System.IO.File.Delete(romPath);
                                        }
                                        
                                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                                        {
                                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                                            System.IO.File.Delete(boxPath);
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
                            var model = BuscarRomPorId(id, token);

                            if (TempData["Unauthorized"] != null)
                            {
                                if (model != null)
                                {
                                    ExcluirRom(model, token);
                                }
                            }
                            else
                            {
                                token = PegarTokenRefreshAtual();
                                if (!string.IsNullOrEmpty(token))
                                {
                                    model = BuscarRomPorId(id, token);
                                    if (TempData["Unauthorized"] != null)
                                    {
                                        if(model != null)
                                        {
                                            ExcluirRom(model, token);
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
                                var model = BuscarRomPorId(id, token);
                                if (TempData["Unauthorized"] != null)
                                {
                                    if (model != null)
                                    {
                                        ExcluirRom(model, token);
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