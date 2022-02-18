using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Administrador.ViewModels;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class RomController : BaseController
    {
        public RomController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        // GET: Rom
        public ActionResult Index(int pagina = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var lista = ListarRoms();
                return View(lista.OrderBy(x => x.Id).ToPagedList(pagina, 4));
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        //POST: Rom/CarregarRoms
        [HttpPost]
        public JsonResult CarregarRoms()
        {
            var obj = new RomTableModel();

            try
            {
                var lista = ListarRoms();

                foreach (var item in lista)
                {
                    obj.data.Add(new RomListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Genero = item.Genero,
                        Created = item.Created,
                        Modified = item.Modified,
                        Novo = item.Novo
                    });
                }

                obj.recordsFiltered = obj.data.Count();
                obj.recordsTotal = obj.data.Count();

                return Json(obj);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                obj.error = ex.Message;
                return Json(obj);
            }
        }

        // GET: Rom/Cadastrar
        public ActionResult Cadastrar()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                return View();
            }

            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Rom/Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(RomViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CadastrarRom(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Rom inserida com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Rom/Detalhes
        [HttpPost]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var model = BuscarRomPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarRom(model);
                    }
                }

                if (errorsList.Count > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, obj = model });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // GET: Rom/Editar
        public ActionResult Editar(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = BuscarRomPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarRom(model);
                        Session.Clear();
                    }
                }
                else
                {
                    throw new HttpException(Convert.ToInt32(HttpStatusCode.NotFound), errorsList.FirstOrDefault());
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Rom/Editar
        [HttpPost]
        public ActionResult Editar(RomViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AtualizarRom(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Rom atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Rom/Excluir
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarRomPorId(id);

                    if (model != null)
                    {
                        ExcluirRom(model);
                    }
                }
                else
                {
                    errorsList.Add("Parâmetros incorretos!");
                }

                if (errorsList.Count > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Rom excluída com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }
    }
}