using Antiguera.WebApi.Controllers.Web.Base;
using System.Web.Mvc;

namespace Antiguera.WebApi.Controllers.Web
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}