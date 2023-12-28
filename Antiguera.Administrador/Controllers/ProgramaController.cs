using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
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
    public class ProgramaController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IProgramaClient _programaClient;

        public ProgramaController(IProgramaClient programaClient)
        {
            _programaClient = programaClient;
        }

        // GET: Programa
        public ActionResult Index()
        {
            return View();
        }

        //POST: Programa/CarregarProgramas
        [HttpPost]
        public async Task<JsonResult> CarregarProgramas()
        {
            var tabela = new ProgramaTableModel();

            try
            {
                tabela = await _programaClient.ListarTabela(UrlConfigurationHelper.ProgramaGetAll);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
            }

            return Json(tabela);
        }

        // POST: Programa/Cadastrar
        [HttpPost]
        public async Task<JsonResult> Cadastrar(ProgramaModel model)
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

                string result = await _programaClient.Inserir(UrlConfigurationHelper.ProgramaCreate, model);

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

        // POST: Programa/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(ProgramaModel model)
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

                string result = await _programaClient.Atualizar(UrlConfigurationHelper.ProgramaEdit, model);

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

        //POST: Programa/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(ProgramaModel model)
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

                string result = await _programaClient.Excluir(UrlConfigurationHelper.ProgramaDelete, model);

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