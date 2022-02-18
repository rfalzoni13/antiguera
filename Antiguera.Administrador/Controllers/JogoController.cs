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
    public class JogoController : BaseController
    {
        public JogoController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        // GET: Jogo
        public ActionResult Index(int pagina = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var lista = ListarJogos();
                return View(lista.OrderBy(x => x.Id).ToPagedList(pagina, 4));
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        //POST: Jogo/CarregarJogos
        [HttpPost]
        public JsonResult CarregarJogos()
        {
            var obj = new JogoTableModel();

            try
            {
                var lista = ListarJogos();
                
                foreach (var item in lista)
                {
                    obj.data.Add(new JogoListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Developer = item.Developer,
                        Publisher = item.Publisher,
                        Genero = item.Genero,
                        Plataforma = item.Plataforma,
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

        // GET: Jogo/Cadastrar
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

        // POST: Jogo/Cadastrar
        [HttpPost]
        public ActionResult Cadastrar(JogoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CadastrarJogo(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Jogo cadastrado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Jogo/Detalhes
        [HttpPost]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var model = BuscarJogoPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarJogo(model);
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

        // GET: Jogo/Editar
        public ActionResult Editar(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = BuscarJogoPorId(id);

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        AtualizarJogo(model);
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

        // POST: Jogo/Editar
        [HttpPost]
        public ActionResult Editar(JogoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AtualizarJogo(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Jogo atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Jogo/Excluir
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarJogoPorId(id);

                    if (model != null)
                    {
                        ExcluirJogo(model);
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

                return Json(new { success = true, message = "Jogo excluído com sucesso!" });
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