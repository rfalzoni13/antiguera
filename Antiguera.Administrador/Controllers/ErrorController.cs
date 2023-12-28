using System;
using System.Net;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class ErrorController : Controller
    {

        // GET: Error
        public ActionResult Index()
        {
            Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            return View();
        }

        //GET: Error/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
            
            return View("NotFound");
        }
    }
}