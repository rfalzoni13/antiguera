using NLog;
using System;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            try
            {
                var user = Request.GetOwinContext().Authentication.User;

                ViewBag.Usuario = user.Identity.Name;

                return View();
            }

            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }
    }
}