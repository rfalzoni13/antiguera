using Antiguera.Administrador.Clients.Interface;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Common;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Utils.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Areas.Cadastro.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IAcessoClient _acessoClient;
        private readonly IUsuarioClient _usuarioClient;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UsuarioController(IAcessoClient acessoClient, IUsuarioClient usuarioClient)
        {
            _acessoClient = acessoClient;
            _usuarioClient = usuarioClient;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        //GET: Usuario/CarregarTabela
        [HttpGet]
        public async Task<JsonResult> CarregarTabela()
        {
            var tabela = new UsuarioTableModel();

            try
            {
                tabela = await _usuarioClient.ListarTabela(UrlConfigurationHelper.UsuarioGetAll);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
            }

            return Json(tabela, JsonRequestBehavior.AllowGet);
        }

        //GET: Usuario/Novo
        [HttpGet]
        public ActionResult Novo()
        {
            var model = new UsuarioModel();

            try
            {
                model.Perfis = _acessoClient.ObterTodosNomesAcessos();

                return View(model);
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);

                TempData["Return"] = new ReturnModel
                {
                    Tipo = "Error",
                    Mensagem = ex.Message
                };

                return View("Index");
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

        // POST: Jogo/Novo
        [HttpPost]
        public async Task<ActionResult> Novo(UsuarioModel model)
        {
            //List<string> errorsList = new List<string>();

            try
            {
                throw new ApplicationException("Erro teste");

                string result = await _usuarioClient.Inserir(UrlConfigurationHelper.UsuarioCreate, model);

                return View(model);
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);

                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
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
            var model = new UsuarioModel();

            try
            {
                model = await _usuarioClient.Listar(UrlConfigurationHelper.UsuarioGet, id);
                model.Perfis = _acessoClient.ObterTodosNomesAcessos();

                return View(model);
            }
            catch (ApplicationException ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);

                TempData["Return"] = new ReturnModel
                {
                    Tipo = "Error",
                    Mensagem = ex.Message
                };

                return View("Index");
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

        // POST: Usuario/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(UsuarioModel model)
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

                string result = await _usuarioClient.Atualizar(UrlConfigurationHelper.UsuarioEdit, model);

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

        //POST: Usuario/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(UsuarioModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                string result = await _usuarioClient.Excluir(UrlConfigurationHelper.UsuarioDelete, model);

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