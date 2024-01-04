using Antiguera.Administrador.Clients.Interface;
using Antiguera.Utils.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Filters
{
    public class DashBoardActionAttribute : ActionFilterAttribute
    {
        private readonly IUsuarioClient _usuarioClient;

        public DashBoardActionAttribute(IUsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.Current.Request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value;

                string token = RequestHelper.GetAccessToken();

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var usuario = Task.Run(async () => await _usuarioClient.Listar($"{UrlConfigurationHelper.UsuarioGet}", userId)).Result;

                filterContext.Controller.ViewBag.Usuario = StringHelper.SetDashboardName(usuario.Nome);
                filterContext.Controller.ViewBag.Perfil = usuario.Acessos[0];
            }

            base.OnActionExecuting(filterContext);
        }
    }
}