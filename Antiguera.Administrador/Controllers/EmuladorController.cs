using Antiguera.Administrador.Models.Tables;
using Antiguera.Administrador.ViewModels;
using Antiguera.Dominio.Interfaces.Servicos;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class EmuladorController : Controller
    {
        private readonly IEmuladorServico _emuladorServico;
        private static Logger _logger;

        public EmuladorController(IEmuladorServico emuladorServico)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _emuladorServico = emuladorServico;
        }

        // GET: Emulador
        public ActionResult Index(int pagina = 1)
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

        //POST: Emulador/CarregarEmuladores
        [HttpPost]
        public JsonResult CarregarEmuladores()
        {
            var obj = new EmuladorTableModel();

            try
            {
                var lista = _emuladorServico.ListarTodos();

                foreach (var item in lista)
                {
                    obj.data.Add(new EmuladorListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Console = item.Console,
                        Roms = item.Roms.Count(),
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


        // GET: Emulador/Cadastrar
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

            catch(Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Emulador/Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(EmuladorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CadastrarEmulador(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Emulador cadastrado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Emulador/Detalhes
        [HttpPost]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var model = BuscarEmuladorPorId(id);

                if (model != null)
                {
                    if(model.Novo == true)
                    {
                        AtualizarEmulador(model);
                    }
                }

                if(errorsList.Count > 0)
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

        // GET: Emulador/Editar
        public ActionResult Editar(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = BuscarEmuladorPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarEmulador(model);
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

        // POST: Emulador/Editar
        [HttpPost]
        public ActionResult Editar(EmuladorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AtualizarEmulador(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Emulador atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Emulador/Excluir
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarEmuladorPorId(id);

                    if (model != null)
                    {
                        ExcluirEmulador(model);
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

                return Json(new { success = true, message = "Emulador excluído com sucesso!" });
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