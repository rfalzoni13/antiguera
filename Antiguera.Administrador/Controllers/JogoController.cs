using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    public class JogoController : BaseController
    {
        // GET: Jogo
        public ActionResult Index(int pagina = 1)
        {
            if (TempData["Mensagem"] != null)
            {
                ViewBag.Mensagem = TempData["Mensagem"];
            }

            if (TempData["ErroMensagem"] != null)
            {
                ViewBag.ErroMensagem = TempData["ErroMensagem"];
            }

            var lista = ListarJogos().OrderBy(j => j.Id).ToPagedList(pagina, 4);
            return View();
        }

        // GET: Cadastrar
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(JogoModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Created = DateTime.Now;

                if (model.FileJogo != null && model.FileJogo.ContentLength > 0)
                {
                    var gameFileName = Path.GetFileName(model.FileJogo.FileName);
                    var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), gameFileName);
                    model.FileJogo.SaveAs(gamePath);
                    model.UrlArquivo = "/Content/Games/" + gameFileName;
                }

                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                    model.FileBoxArt.SaveAs(boxPath);
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                }

                if (CadastrarJogo(model))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            JogoModel model = new JogoModel();

            model = BuscarJogoPorId(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Editar
        [HttpPost]
        public ActionResult Editar(JogoModel model)
        {
            if (ModelState.IsValid)
            {
                model.Modified = DateTime.Now;
                if (AtualizarJogo(model))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Excluir
        public ActionResult Excluir(int id)
        {
            if (id == 0)
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
            }
            else
            {
                var model = BuscarJogoPorId(id);
                if(model != null)
                {
                    ExcluirJogo(model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}