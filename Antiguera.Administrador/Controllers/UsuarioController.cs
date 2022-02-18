using Antiguera.Administrador.Models.Tables;
using Antiguera.Administrador.ViewModels;
using Antiguera.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        public UsuarioController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            : base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        // GET: Usuario
        public ActionResult Index(int pagina = 1)
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

        //POST: Usuario/CarregarUsuarios
        [HttpPost]
        public JsonResult CarregarUsuarios()
        {
            var obj = new UsuarioTableModel();

            try
            {
                var lista = ListarUsuarios();

                foreach (var item in lista)
                {
                    obj.data.Add(new UsuarioListTableModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Email = item.Email,
                        Login = item.Login,
                        Sexo = item.Sexo,
                        Created = item.Created,
                        Modified = item.Modified,
                        Novo = item.Novo
                    });
                }

                obj.recordsFiltered = obj.data.Count();
                obj.recordsTotal = obj.data.Count();

                return Json(obj);
            }
            catch(Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                obj.error = ex.Message;
                return Json(obj);
            }
        }

        // GET: Usuario/Cadastrar
        public ActionResult Cadastrar()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = new UsuarioViewModel();
                model.ListaAcessos = new List<SelectListItem>();

                model.ListaAcessos.Add(new SelectListItem() { Text = "Selecione uma opção...", Value = "0" });

                var acessos = ListarAcessos();

                if(acessos.Count() > 0)
                {
                    foreach (var acesso in acessos)
                    {
                        model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                    }
                }
                    
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Usuario/Cadastrar
        [HttpPost]
        public async Task<ActionResult> Cadastrar(UsuarioViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await CadastrarUsuario(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }
                if (errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Usuário inserido com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // GET: Usuario/Editar
        public async Task<ActionResult> Editar(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = BuscarUsuarioPorId(id);
                model.ListaAcessos = new List<SelectListItem>();

                model.ListaAcessos.Add(new SelectListItem() { Text = "Selecione uma opção...", Value = "0" });

                var acessos = ListarAcessos();

                if (model != null)
                {
                    if (model.Novo == true)
                    {
                        await AtualizarUsuario(model);
                    }

                    if(acessos.Count() > 0)
                    {
                        foreach (var acesso in acessos)
                        {
                            model.ListaAcessos.Add(new SelectListItem() { Text = acesso.Nome, Value = acesso.Id.ToString() });
                        }
                    }

                    return View(model);
                }
                else
                {
                    throw new HttpException(Convert.ToInt32(HttpStatusCode.NotFound), errorsList.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Usuario/Editar
        [HttpPost]
        public async Task<ActionResult> Editar(UsuarioViewModel model)
        {
            try
            {
                var usuario = BuscarUsuarioPorId(model.Id);
                if (usuario != null)
                {
                    model.Foto = usuario.Foto;
                    model.Created = usuario.Created;
                }

                ModelState.Remove("Senha");
                ModelState.Remove("ConfirmarSenha");

                if (ModelState.IsValid)
                {
                    await AtualizarUsuario(model);
                }
                else
                {
                    AdicionarModelStateErrors(ModelState);
                }

                if(errorsList.Count() > 0)
                {
                    return Json(new { success = false, errors = errorsList });
                }

                return Json(new { success = true, message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
                return Json(new { success = false, errors = errorsList });
            }
        }

        // POST: Usuario/Excluir
        [HttpPost]
        public async Task<ActionResult> Excluir(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = BuscarUsuarioPorId(id);

                    if (model != null)
                    {
                        await ExcluirUsuario(model);
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

                return Json(new { success = true, message = "Usuário excluído com sucesso!" });
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