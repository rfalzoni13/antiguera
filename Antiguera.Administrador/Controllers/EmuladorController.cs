using Antiguera.Administrador.Client.Interface;
using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class EmuladorController : BaseController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IEmuladorClient _emuladorClient;

        public EmuladorController(IEmuladorClient emuladorClient)
        {
            _emuladorClient = emuladorClient;
        }

        // GET: Emulador
        public ActionResult Index()
        {
            return View();
        }

        //POST: Emulador/CarregarEmuladors
        [HttpPost]
        public async Task<JsonResult> CarregarEmuladors()
        {
            var obj = new EmuladorTableModel();

            try
            {
                var url = UrlConfiguration.EmuladorGetAll;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var emuladors = await _emuladorClient.ListarTodos(url, token);

                foreach (var emulador in emuladors)
                {
                    obj.data.Add(new EmuladorListTableModel()
                    {
                        Id = emulador.Id,
                        Nome = emulador.Nome,
                        Console = emulador.Console,
                        Roms = emulador.Roms.Count(),
                        Created = emulador.Created,
                        Modified = emulador.Modified,
                        Novo = emulador.Novo
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

                if (Debugger.IsAttached)
                {
                    obj.error = "Ocorreu um erro ao processar a solicitação!";
                }

                return Json(obj);
            }
        }

        // POST: Emulador/Cadastrar
        [HttpPost]
        public async Task<JsonResult> Cadastrar(EmuladorModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            errorsList.Add(error.ErrorMessage);
                        }
                    }

                    return Json(new { success = false, errors = errorsList });
                }

                var url = UrlConfiguration.EmuladorCreate;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _emuladorClient.Inserir(url, token, model);

                return Json(new { success = true, message = result });
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                errorsList.Add(ex.Message);

                return Json(new { success = false, errors = errorsList });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);

                errorsList.Add(ex.Message);

                if (Debugger.IsAttached)
                {
                    errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                }

                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Emulador/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(EmuladorModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            errorsList.Add(error.ErrorMessage);
                        }
                    }

                    return Json(new { success = false, errors = errorsList });
                }

                var url = UrlConfiguration.EmuladorEdit;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _emuladorClient.Atualizar(url, token, model);

                return Json(new { success = true, message = result });

            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                errorsList.Add(ex.Message);

                return Json(new { success = false, errors = errorsList });
            }

            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);

                errorsList.Add(ex.Message);

                if (Debugger.IsAttached)
                {
                    errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                }

                return Json(new { success = false, errors = errorsList });
            }
        }

        //POST: Emulador/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(EmuladorModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            errorsList.Add(error.ErrorMessage);
                        }
                    }

                    return Json(new { success = false, errors = errorsList });
                }

                var url = UrlConfiguration.EmuladorDelete;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _emuladorClient.Excluir(url, token, model);

                return Json(new { success = true, message = result });
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                errorsList.Add(ex.Message);

                return Json(new { success = false, errors = errorsList });
            }

            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);

                errorsList.Add(ex.Message);

                if (Debugger.IsAttached)
                {
                    errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                }

                return Json(new { success = false, errors = errorsList });
            }
        }
    }
}