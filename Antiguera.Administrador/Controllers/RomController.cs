using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
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

                    var lista = ListarRoms();
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

                        CadastrarRom(model);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        return View("Index");
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
                    var model = BuscarRomPorId(id);
                    if (Session["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }

                    if (model != null)
                    {
                        if (model.Novo == true)
                        {
                            AtualizarRom(model);
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
                    Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                    HttpContext.GetOwinContext().Authentication.SignOut();
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
                    var model = BuscarRomPorId(id);
                    if (Session["Unauthorized"] != null)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        return RedirectToAction("Login", "Home");
                    }

                    if (model != null)
                    {
                        if (model.Novo == true)
                        {
                            AtualizarRom(model);
                            Session.Clear();
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
                    Session["ErroMensagem"] = "Sua sessão expirou! Faça login novamente!";
                    HttpContext.GetOwinContext().Authentication.SignOut();
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

                        AtualizarRom(model);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        return View("Index");
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
                        var model = BuscarRomPorId(id);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        if(model != null)
                        {
                            ExcluirRom(model);
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