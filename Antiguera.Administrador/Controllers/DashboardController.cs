using Antiguera.Administrador.Controllers.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        // POST: ListarUsuarios
        [HttpPost]
        public ActionResult CarregarInformações()
        {
            try
            {
                var usuarios = ListarUsuarios();
                var jogos = ListarJogos();
                var programas = ListarProgramas();

                return Json(new
                {
                    success = true,
                    users = usuarios,
                    games = jogos,
                    programs = programas
                });
            }
            catch(Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}