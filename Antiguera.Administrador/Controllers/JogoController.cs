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
    public class JogoController : Controller
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
            var tabela = new JogoTableModel();

            try
            {
                tabela = await _jogoClient.ListarTabela(UrlConfigurationHelper.JogoGetAll);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
            }

            return Json(tabela);
        }

        //GET: Jogo/Cadastrar
        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View(new JogoModel());
        }

        // POST: Jogo/Cadastrar
        [HttpPost]
        public async Task<ActionResult> Cadastrar(JogoModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                string result = await _jogoClient.Inserir(UrlConfigurationHelper.JogoCreate, model);

                return View();
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);

                ModelState.AddModelError(string.Empty, ex.Message);

                return View();
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);

#if !DEBUG
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, verifique o arquivo de log e tente novamente!");
#else
                ModelState.AddModelError(string.Empty, ex.Message);
#endif
                throw;
            }
        }

        // GET: Jogo/Editar
        [HttpGet]
        public async Task<ActionResult> Editar(string id)
        {
            var model = await _jogoClient.Listar(UrlConfigurationHelper.JogoGet, id);

            return View(model);
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

                string result = await _jogoClient.Atualizar(UrlConfigurationHelper.JogoEdit, model);

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

                string result = await _jogoClient.Excluir(UrlConfigurationHelper.JogoDelete, model);

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