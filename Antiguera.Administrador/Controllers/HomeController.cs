using Antiguera.Administrador.Client.Interface;
using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Helpers;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUsuarioClient _usuarioClient;

        public HomeController(IUsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                string userId = Request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value;

                string token = Session["Token"] != null ? Session["Token"].ToString() : null;

                if (string.IsNullOrEmpty(token)) throw new Exception("Não autorizado!");

                var usuario = await _usuarioClient.ListarPorIdentityId(userId, token);

                ViewBag.Usuario = BuilderString.SetDashboardName(usuario.Nome);
                ViewBag.Perfil = usuario.Acesso.Nome;

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