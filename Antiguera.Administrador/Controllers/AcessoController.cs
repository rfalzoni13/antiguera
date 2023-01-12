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
    public class AcessoController : BaseController
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
            var obj = new AcessoTableModel();

            try
            {
                var url = UrlConfiguration.AcessoGetAll;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var acessos = await _acessoClient.ListarTodos(url, token);

                foreach (var acesso in acessos)
                {
                    obj.data.Add(new AcessoListTableModel()
                    {
                        Id = acesso.Id,
                        Nome = acesso.Nome,
                        Created = acesso.Created,
                        Modified = acesso.Modified,
                        Novo = acesso.Novo
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
                
                if(Debugger.IsAttached)
                {
                    obj.error = "Ocorreu um erro ao processar a solicitação!";
                }
                
                return Json(obj);
            }
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

                var url = UrlConfiguration.AcessoCreate;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _acessoClient.Inserir(url, token, model);

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

                var url = UrlConfiguration.AcessoEdit;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _acessoClient.Atualizar(url, token, model);

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

                var url = UrlConfiguration.AcessoDelete;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _acessoClient.Excluir(url, token, model);

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