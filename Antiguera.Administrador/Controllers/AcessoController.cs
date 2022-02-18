using Antiguera.Administrador.Models.Tables;
using Antiguera.Administrador.ViewModels;
using Antiguera.Servicos.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class AcessoController : Controller
    {
        private readonly IAcessoServico _acessoServico;

        public AcessoController(IAcessoServico acessoServico)
        {
            _acessoServico = acessoServico;
        }

        // GET: Acesso
        public ActionResult Index()
        {
            return View();
        }

        //POST: Acesso/CarregarAcessos
        [HttpPost]
        public JsonResult CarregarAcessos()
        {
            var obj = new AcessoTableModel();

            try
            {
                var lista = _acessoServico.ListarTodos();

                foreach (var item in lista)
                {
                    obj.data.Add(new AcessoListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
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

        // POST: Acesso/Cadastrar
        [HttpPost]
        public async Task<ActionResult> Cadastrar(AcessoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await CadastrarAcesso(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Acesso incluído com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Acesso/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(AcessoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await AtualizarAcesso(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Acesso atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        //POST: Acesso/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarAcessoPorId(id);

                    if (model != null)
                    {
                        await ExcluirAcesso(model);
                    }
                }
                else
                {
                    errorsList.Add("Parâmetros incorretos!");
                }

                if(errorsList.Count > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Acesso excluído com sucesso!" });
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