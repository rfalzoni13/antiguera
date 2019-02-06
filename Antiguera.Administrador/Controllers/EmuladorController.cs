using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    public class EmuladorController : BaseController
    {
        // GET: Emulador
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

            var lista = ListarEmuladores().OrderBy(j => j.Id).ToPagedList(pagina, 4);
            return View();
        }

        // GET: Cadastrar
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(EmuladorModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Created = DateTime.Now;

                if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                {
                    var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                    var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                    model.FileEmulador.SaveAs(emuPath);
                    model.UrlArquivo = "/Content/Consoles/Emuladores/" + emuladorFileName;
                }

                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                    model.FileBoxArt.SaveAs(boxPath);
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                }

                if (CadastrarEmulador(model))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            EmuladorModel model = new EmuladorModel();

            model = BuscarEmuladorPorId(id);
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
        public ActionResult Editar(EmuladorModel model)
        {
            if (ModelState.IsValid)
            {
                model.Modified = DateTime.Now;
                if (AtualizarEmulador(model))
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
                var model = BuscarEmuladorPorId(id);
                if (model != null)
                {
                    ExcluirEmulador(model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}