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
    public class JogoController : BaseController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IJogoClient _jogoClient;

        public JogoController(IJogoClient jogoClient)
        {
            _jogoClient = jogoClient;
        }

        // GET: Jogo
        public ActionResult Index()
        {
            return View();
        }

        //POST: Jogo/CarregarJogos
        [HttpPost]
        public async Task<JsonResult> CarregarJogos()
        {
            var obj = new JogoTableModel();

            try
            {
                var url = UrlConfiguration.JogoGetAll;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var jogos = await _jogoClient.ListarTodos(url, token);

                foreach (var jogo in jogos)
                {
                    obj.data.Add(new JogoListTableModel()
                    {
                        Id = jogo.Id,
                        Nome = jogo.Nome,
                        Created = jogo.Created,
                        Modified = jogo.Modified,
                        Novo = jogo.Novo
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

        // POST: Jogo/Cadastrar
        [HttpPost]
        public async Task<JsonResult> Cadastrar(JogoModel model)
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

                var url = UrlConfiguration.JogoCreate;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _jogoClient.Inserir(url, token, model);

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

        // POST: Jogo/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(JogoModel model)
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

                var url = UrlConfiguration.JogoEdit;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _jogoClient.Atualizar(url, token, model);

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

        //POST: Jogo/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(JogoModel model)
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

                var url = UrlConfiguration.JogoDelete;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _jogoClient.Excluir(url, token, model);

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