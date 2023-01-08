using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
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