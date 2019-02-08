using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class ProgramaController : BaseController
    {
        // GET: Programa
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

            var lista = ListarProgramas().OrderBy(j => j.Id).ToPagedList(pagina, 4);
            return View();
        }

        // GET: Cadastrar
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(ProgramaModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Created = DateTime.Now;

                if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                {
                    var programaFileName = Path.GetFileName(model.FilePrograma.FileName);
                    var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                    model.FilePrograma.SaveAs(progPath);
                    model.UrlArquivo = "/Content/Programas/" + programaFileName;
                }

                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                    model.FileBoxArt.SaveAs(boxPath);
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                }

                if (CadastrarPrograma(model))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            ProgramaModel model = new ProgramaModel();

            model = BuscarProgramaPorId(id);
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
        public ActionResult Editar(ProgramaModel model)
        {
            if (ModelState.IsValid)
            {
                model.Modified = DateTime.Now;
                if (AtualizarPrograma(model))
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
                var model = BuscarProgramaPorId(id);
                if (model != null)
                {
                    ExcluirPrograma(model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}