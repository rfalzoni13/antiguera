using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    public class AcessoController : BaseController
    {
        // GET: Acesso
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

                    var lista = ListarAcessos();
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

        //GET: Pesquisa
        //public ActionResult Pesquisa(string busca, int pagina = 1)
        //{
        //    var retorno = PesquisarAcessos(busca);
        //    if (retorno != null && retorno.Count > 0)
        //    {
        //        return View(retorno.OrderBy(x => x.AcessoId).ToPagedList(pagina, 4));

        //    }
        //    else
        //    {
        //        Session["ErroMensagem"] = "Não foi encontrado nenhum registro de acordo com sua pesquisa!";
        //        return RedirectToAction("Index");
        //    }
        //}

        // POST: Editar Acesso
        [HttpPost]
        public ActionResult Editar(AcessoModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(model.Nome))
                    {
                        var nome = model.Nome;

                        model = BuscarAcessoPorId(model.Id);
                        model.Nome = nome;
                        AtualizarAcesso(model);
                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        Session["ErroMensagem"] = "Verifique todos os campos!";
                    }
                }
                return RedirectToAction("Index");
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

        // POST: Salvar Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(AcessoModel model)
        {
            try
            {
                if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    if (model != null && ModelState.IsValid)
                    {
                        CadastrarAcesso(model);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        Session["ErroMensagem"] = "Po favor, verifique todos os campos";
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

        //GET Excluir
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
                        var model = BuscarAcessoPorId(id);

                        if (Session["Unauthorized"] != null)
                        {
                            HttpContext.GetOwinContext().Authentication.SignOut();
                            return RedirectToAction("Login", "Home");
                        }

                        if (model != null)
                        {
                            ExcluirAcesso(model);
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