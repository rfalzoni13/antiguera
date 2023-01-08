using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class DashboardController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        // POST: ListarUsuarios
        [HttpPost]
        public async Task<ActionResult> CarregarInformações()
        {
            var obj = new DashboardModel();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = UrlConfiguration.VerifyCode;


                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<DashboardModel>();

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
    }
}