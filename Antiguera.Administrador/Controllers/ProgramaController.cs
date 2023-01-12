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
    public class ProgramaController : BaseController
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
            var obj = new ProgramaTableModel();

            try
            {
                var url = UrlConfiguration.ProgramaGetAll;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var programas = await _programaClient.ListarTodos(url, token);

                foreach (var programa in programas)
                {
                    obj.data.Add(new ProgramaListTableModel()
                    {
                        Id = programa.Id,
                        Nome = programa.Nome,
                        Created = programa.Created,
                        Modified = programa.Modified,
                        Novo = programa.Novo
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

                var url = UrlConfiguration.ProgramaCreate;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _programaClient.Inserir(url, token, model);

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

                var url = UrlConfiguration.ProgramaEdit;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _programaClient.Atualizar(url, token, model);

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

                var url = UrlConfiguration.ProgramaDelete;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _programaClient.Excluir(url, token, model);

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