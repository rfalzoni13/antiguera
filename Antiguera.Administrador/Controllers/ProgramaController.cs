using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Models.Tables;
using Antiguera.Administrador.ViewModels;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class ProgramaController : BaseController
    {
        public ProgramaController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        // GET: Programa
        public ActionResult Index(int pagina = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var lista = ListarProgramas();
                return View(lista.OrderBy(x => x.Id).ToPagedList(pagina, 4));
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        //POST: Programa/CarregarProgramas
        [HttpPost]
        public JsonResult CarregarProgramas()
        {
            var obj = new ProgramaTableModel();

            try
            {
                var lista = ListarProgramas();

                foreach (var item in lista)
                {
                    obj.data.Add(new ProgramaListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Developer = item.Developer,
                        Publisher = item.Publisher,
                        Tipo = item.Tipo,
                        Created = item.Created,
                        Modified = item.Modified,
                        Novo = item.Novo
                    });
                }

                obj.recordsFiltered = obj.data.Count();
                obj.recordsTotal = obj.data.Count();

                return Json(obj);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                obj.error = ex.Message;
                return Json(obj);
            }
        }


        // GET: Programa/Cadastrar
        public ActionResult Cadastrar()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

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

        // POST: Programa/Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(ProgramaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CadastrarPrograma(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Programa inserido com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Programa/Detalhes
        [HttpPost]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var model = BuscarProgramaPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarPrograma(model);
                    }
                }

                if (errorsList.Count > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, obj = model });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // GET: Programa/Editar
        public ActionResult Editar(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = BuscarProgramaPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarPrograma(model);
                        Session.Clear();
                    }
                }
                else
                {
                    throw new HttpException(Convert.ToInt32(HttpStatusCode.NotFound), errorsList.FirstOrDefault());
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }


        // POST: Programa/Editar
        [HttpPost]
        public ActionResult Editar(ProgramaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AtualizarPrograma(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Programa atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Programa/Excluir
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarProgramaPorId(id);

                    if (model != null)
                    {
                        ExcluirPrograma(model);
                    }
                }
                else
                {
                    errorsList.Add("Parâmetros incorretos!");
                }

                if (errorsList.Count > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Programa excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }
    }
}