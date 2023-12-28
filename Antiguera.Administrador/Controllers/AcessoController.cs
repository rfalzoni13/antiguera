using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class AcessoController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IAcessoClient _acessoClient;

        public AcessoController(IAcessoClient acessoClient)
        {
            _acessoClient = acessoClient;
        }

        // GET: Acesso
        public ActionResult Index()
        {
            return View();
        }

        //POST: Acesso/CarregarAcessos
        [HttpPost]
        public async Task<JsonResult> CarregarAcessos()
        {
            var tabela = new AcessoTableModel();

            try
            {
                tabela = await _acessoClient.ListarTabela(UrlConfigurationHelper.AcessoGetAll);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
            }

            return Json(tabela);
        }

        // POST: Acesso/Cadastrar
        [HttpPost]
        public async Task<JsonResult> Cadastrar(AcessoModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach(var modelState in ModelState.Values)
                    {
                        foreach(var error in modelState.Errors)
                        {
                            errorsList.Add(error.ErrorMessage);
                        }
                    }

                    return Json(new { success = false, errors = errorsList });
                }

                string result = await _acessoClient.Inserir(UrlConfigurationHelper.AcessoCreate, model);

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
                
                if(Debugger.IsAttached)
                {
                    errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                }

                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Acesso/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(AcessoModel model)
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

                string result = await _acessoClient.Atualizar(UrlConfigurationHelper.AcessoEdit, model);

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

        //POST: Acesso/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(AcessoModel model)
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

                string result = await _acessoClient.Excluir(UrlConfigurationHelper.AcessoDelete, model);

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