using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using Antiguera.Administrador.Models.Tables;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class ProgramaController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.VerifyCode;


                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<ICollection<ProgramaModel>>();

                        foreach (var item in result)
                        {
                            obj.data.Add(new ProgramaListTableModel()
                            {
                                Id = item.Id,
                                Nome = item.Nome,
                                Created = item.Created,
                                Modified = item.Modified,
                                Novo = item.Novo
                            });
                        }

                        obj.recordsFiltered = obj.data.Count();
                        obj.recordsTotal = obj.data.Count();

                        return Json(obj);
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                obj.error = ex.Message;
#if !DEBUG
                obj.error = "Ocorreu um erro ao processar a solicitação!";
#endif
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

                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.SendCode;


                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        return Json(new { success = true, message = result });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add(ex.Message);
#if !DEBUG
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
#endif

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

                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.SendCode;


                    HttpResponseMessage response = await client.PostAsJsonAsync(url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        return Json(new { success = true, message = result });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add(ex.Message);
#if !DEBUG
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
#endif

                return Json(new { success = false, errors = errorsList });
            }
        }

        //POST: Programa/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(int id)
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

                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.Login;

                    string queryId = id.ToString();

                    string param = $"acessoId={queryId}";

                    HttpContent content = new StringContent(param, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        return Json(new { success = true, message = result });
                    }
                    else
                    {
                        StatusCodeModel statusCode = response.Content.ReadAsAsync<StatusCodeModel>().Result;

                        throw new Exception(statusCode.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add(ex.Message);
#if !DEBUG
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
#endif

                return Json(new { success = false, errors = errorsList });
            }
        }
    }
}