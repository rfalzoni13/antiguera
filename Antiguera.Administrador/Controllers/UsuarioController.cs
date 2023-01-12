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
    public class UsuarioController : BaseController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUsuarioClient _usuarioClient;

        public UsuarioController(IUsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        //POST: Usuario/CarregarUsuarios
        [HttpPost]
        public async Task<JsonResult> CarregarUsuarios()
        {
            var obj = new UsuarioTableModel();

            try
            {
                var url = UrlConfiguration.UsuarioGetAll;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var usuarios = await _usuarioClient.ListarTodos(url, token);

                foreach (var usuario in usuarios)
                {
                    obj.data.Add(new UsuarioListTableModel()
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Created = usuario.Created,
                        Modified = usuario.Modified,
                        Novo = usuario.Novo
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

        // POST: Usuario/Cadastrar
        [HttpPost]
        public async Task<JsonResult> Cadastrar(UsuarioModel model)
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

                var url = UrlConfiguration.UsuarioCreate;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _usuarioClient.Inserir(url, token, model);

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

                var url = UrlConfiguration.UsuarioEdit;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _usuarioClient.Atualizar(url, token, model);

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

                var url = UrlConfiguration.UsuarioDelete;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                string result = await _usuarioClient.Excluir(url, token, model);

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