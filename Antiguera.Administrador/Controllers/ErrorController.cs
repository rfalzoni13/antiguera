using Antiguera.Administrador.Controllers.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Net;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

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