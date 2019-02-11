using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    public class RomController : BaseController
    {
        // GET: Rom
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

            var lista = ListarRoms().OrderBy(j => j.Id).ToPagedList(pagina, 4);
            return View();
        }

        // GET: Cadastrar
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(RomModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Created = DateTime.Now;

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

                if (CadastrarRom(model))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            var model = BuscarRomPorId(id);
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
        public ActionResult Editar(RomModel model)
        {
            if (ModelState.IsValid)
            {
                model.Modified = DateTime.Now;
                if (AtualizarRom(model))
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
                var model = BuscarRomPorId(id);
                if (model != null)
                {
                    ExcluirRom(model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}