using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers.Base
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated && Session["Token"] == null)
            {
                Session["Token"] = Request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type.Contains("AccessToken")).Value.Split(' ').LastOrDefault();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}