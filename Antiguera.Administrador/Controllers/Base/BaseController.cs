using Antiguera.Administrador.Config;
using Antiguera.Administrador.DTOs;
using Antiguera.Administrador.Models;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers.Base
{
    public class BaseController : Controller
    {
        #region Atributos
        protected HttpClient Cliente = new HttpClient();
        protected UrlConfiguration url = new UrlConfiguration();
        #endregion

        #region Header
        [Authorize]
        public ActionResult _Header()
        {
            HeaderModel model = new HeaderModel();

            var login = HttpContext.Request.GetOwinContext().Authentication.User.Identity.Name;

            if(!string.IsNullOrEmpty(login))
            {
                model.Usuario = BuscarUsuarioPorLoginOuEmail(login);

                model.Usuario.Acesso = BuscarAcessoPorId(model.Usuario.AcessoId);
            }

            model.Usuarios = ListarUsuarios().Where(x => x.Novo == true).ToList();

            model.Jogos = ListarJogos().Where(x => x.Novo == true).ToList();

            model.Emuladores = ListarEmuladores().Where(x => x.Novo == true).ToList();

            model.Roms = ListarRoms().Where(x => x.Novo == true).ToList();

            model.Programas = ListarProgramas().Where(x => x.Novo == true).ToList();
                       
            return PartialView(model);
        }
        #endregion

        #region Módulo Usuário
        [NonAction]
        protected List<UsuarioModel> ListarUsuarios()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosUsuarios).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<UsuarioModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosUsuarios).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<UsuarioModel>>().Result;
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<UsuarioModel>();
        }

        [NonAction]
        protected UsuarioModel BuscarUsuarioPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<UsuarioModel>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<UsuarioModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected UsuarioModel BuscarUsuarioPorLoginOuEmail(string userData)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + userData).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<UsuarioModel>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + userData).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<UsuarioModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarUsuario(UsuarioModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.NomeFoto))
                {
                    model.UrlFotoUpload = "/Content/Images/Profile/" + model.NomeFoto;
                }

                var usuarioDTO = Mapper.Map<UsuarioModel, UsuarioDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirUsuario, usuarioDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                    {
                        var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                        var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                        model.FileFoto.SaveAs(photoPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirUsuario, usuarioDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                        {
                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                            model.FileFoto.SaveAs(photoPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            { 
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarUsuario(UsuarioModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlFotoUpload))
                    {
                        var fileName = model.UrlFotoUpload.Split('/')[4];
                        var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fileName);
                        if (System.IO.File.Exists(photoPath))
                        {
                            System.IO.File.Delete(photoPath);
                        }
                        else
                        {
                            model.UrlFotoUpload = null;
                        }
                    }
                    model.UrlFotoUpload = "/Content/Images/Profile/" + model.NomeFoto;
                }

                var usuarioDTO = Mapper.Map<UsuarioModel, UsuarioDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarUsuario, usuarioDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                    {
                        var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                        var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                        model.FileFoto.SaveAs(photoPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarUsuario, usuarioDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileFoto != null && model.FileFoto.ContentLength > 0)
                        {
                            var fotoFileName = Path.GetFileName(model.FileFoto.FileName);
                            var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), fotoFileName);
                            model.FileFoto.SaveAs(photoPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarSenha(UsuarioModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenha, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;  
                }

                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenha, model).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }

                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                
            }
        }

        [NonAction]
        protected void ExcluirUsuario(UsuarioModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.UrlFotoUpload))
                {
                    var photoPath = Path.Combine(Server.MapPath("~/Content/Images/Profile/"), model.UrlFotoUpload);
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirUsuario)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }

                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
        }
        #endregion

        #region Módulo Permissões
        [NonAction]
        protected List<AcessoModel> ListarAcessos()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosAcessos).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<AcessoModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosAcessos).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<AcessoModel>>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<AcessoModel>();
        }

        [NonAction]
        protected AcessoModel BuscarAcessoPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarAcessoPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<AcessoModel>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarAcessoPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<AcessoModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarAcesso(AcessoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                var acessoDTO = Mapper.Map<AcessoModel, AcessoDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirAcesso, acessoDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirAcesso, acessoDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }
            }
        }

        [NonAction]
        protected void AtualizarAcesso(AcessoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                var acessoDTO = Mapper.Map<AcessoModel, AcessoDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarAcesso, acessoDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlInserirAcesso, acessoDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }
            }
        }

        [NonAction]
        protected void ExcluirAcesso(AcessoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirAcesso)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                }

                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
        }
        #endregion

        #region Módulo Emulador
        [NonAction]
        protected List<EmuladorModel> ListarEmuladores()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosEmuladores).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<EmuladorModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosEmuladores).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<EmuladorModel>>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<EmuladorModel>();
        }

        [NonAction]
        protected EmuladorModel BuscarEmuladorPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarEmuladorPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<EmuladorModel>().Result;

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarEmuladorPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<EmuladorModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }

            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarEmulador(EmuladorModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.NomeArquivo))
                {
                    model.UrlArquivo = "/Content/Consoles/Emuladores/" + model.NomeArquivo;
                }

                var emuladorDTO = Mapper.Map<EmuladorModel, EmuladorDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirEmulador, emuladorDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                    {
                        var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                        var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                        model.FileEmulador.SaveAs(emuPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirEmulador, emuladorDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                        {
                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                            model.FileEmulador.SaveAs(emuPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarEmulador(EmuladorModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlArquivo))
                    {
                        var emuladorFileName = model.UrlArquivo.Split('/')[4];
                        var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                        if (System.IO.File.Exists(emuPath))
                        {
                            System.IO.File.Delete(emuPath);
                        }
                        else
                        {
                            model.UrlArquivo = null;
                        }
                    }
                    model.UrlArquivo = "/Content/Consoles/Emuladores/" + model.NomeArquivo;
                }

                var emuladorDTO = Mapper.Map<EmuladorModel, EmuladorDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarEmulador, emuladorDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                    {
                        var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                        var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                        model.FileEmulador.SaveAs(emuPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarEmulador, emuladorDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileEmulador != null && model.FileEmulador.ContentLength > 0)
                        {
                            var emuladorFileName = Path.GetFileName(model.FileEmulador.FileName);
                            var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), emuladorFileName);
                            model.FileEmulador.SaveAs(emuPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void ExcluirEmulador(EmuladorModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.UrlArquivo))
                {
                    var emuPath = Path.Combine(Server.MapPath("~/Content/Consoles/Emuladores/"), model.UrlArquivo);
                    if (System.IO.File.Exists(emuPath))
                    {
                        System.IO.File.Delete(emuPath);
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirEmulador)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }
        #endregion

        #region Módulo Rom
        [NonAction]
        protected List<RomModel> ListarRoms()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodasRoms).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<RomModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodasRoms).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<RomModel>>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<RomModel>();
        }

        [NonAction]
        protected RomModel BuscarRomPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarRomPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<RomModel>().Result;

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarRomPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<RomModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarRom(RomModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.NomeArquivo))
                {
                    model.UrlArquivo = "/Content/Consoles/Roms/" + model.NomeArquivo;
                }

                if (!string.IsNullOrEmpty(model.NomeFoto))
                {
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                var romDTO = Mapper.Map<RomModel, RomDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirRom, romDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileRom != null && model.FileRom.ContentLength > 0)
                    {
                        var romFileName = Path.GetFileName(model.FileRom.FileName);
                        var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                        model.FileRom.SaveAs(romPath);
                    }

                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirRom, romDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileRom != null && model.FileRom.ContentLength > 0)
                        {
                            var romFileName = Path.GetFileName(model.FileRom.FileName);
                            var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                            model.FileRom.SaveAs(romPath);
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;

                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarRom(RomModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlBoxArt))
                    {
                        var fileName = model.UrlBoxArt.Split('/')[4];
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), fileName);
                        if (System.IO.File.Exists(boxPath))
                        {
                            System.IO.File.Delete(boxPath);
                        }
                        else
                        {
                            model.UrlBoxArt = null;
                        }
                    }
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                if (model.FileRom != null && model.FileRom.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlArquivo))
                    {
                        var fileName = model.UrlArquivo.Split('/')[4];
                        var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), fileName);
                        if (System.IO.File.Exists(romPath))
                        {
                            System.IO.File.Delete(romPath);
                        }
                        else
                        {
                            model.UrlArquivo = null;
                        }
                    }
                    model.UrlArquivo = "/Content/Consoles/Roms/" + model.NomeArquivo;
                }

                var romDTO = Mapper.Map<RomModel, RomDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarRom, romDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileRom != null && model.FileRom.ContentLength > 0)
                    {
                        var romFileName = Path.GetFileName(model.FileRom.FileName);
                        var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                        model.FileRom.SaveAs(romPath);
                    }

                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarRom, romDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileRom != null && model.FileRom.ContentLength > 0)
                        {
                            var romFileName = Path.GetFileName(model.FileRom.FileName);
                            var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), romFileName);
                            model.FileRom.SaveAs(romPath);
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void ExcluirRom(RomModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.UrlArquivo))
                {
                    var romPath = Path.Combine(Server.MapPath("~/Content/Consoles/Roms/"), model.UrlArquivo);
                    if (System.IO.File.Exists(romPath))
                    {
                        System.IO.File.Delete(romPath);
                    }
                }

                if (!string.IsNullOrEmpty(model.UrlBoxArt))
                {
                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), model.UrlBoxArt);
                    if (System.IO.File.Exists(boxPath))
                    {
                        System.IO.File.Delete(boxPath);
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirRom)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }
        #endregion

        #region Módulo Jogo
        [NonAction]
        protected List<JogoModel> ListarJogos()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosJogos).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<JogoModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosJogos).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<JogoModel>>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<JogoModel>();
        }

        [NonAction]
        protected JogoModel BuscarJogoPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarJogoPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<JogoModel>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarJogoPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<JogoModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarJogo(JogoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if(!string.IsNullOrEmpty(model.NomeArquivo))
                {
                    model.UrlArquivo = "/Content/Games/" + model.NomeArquivo;
                }

                if(!string.IsNullOrEmpty(model.NomeFoto))
                {
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                var jogoDTO = Mapper.Map<JogoModel, JogoDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirJogo, jogoDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileJogo != null && model.FileJogo.ContentLength > 0)
                    {
                        var gameFileName = Path.GetFileName(model.FileJogo.FileName);
                        var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), gameFileName);
                        model.FileJogo.SaveAs(gamePath);
                    }
                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirJogo, jogoDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileJogo != null && model.FileJogo.ContentLength > 0)
                        {
                            var gameFileName = Path.GetFileName(model.FileJogo.FileName);
                            var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), gameFileName);
                            model.FileJogo.SaveAs(gamePath);
                            model.UrlArquivo = "/Content/Games/" + gameFileName;
                        }
                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                            model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }

                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarJogo(JogoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if(model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    if(!string.IsNullOrEmpty(model.UrlBoxArt))
                    {
                        var fileName = model.UrlBoxArt.Split('/')[4];
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), fileName);
                        if(System.IO.File.Exists(boxPath))
                        {
                            System.IO.File.Delete(boxPath);
                        }
                        else
                        {
                            model.UrlBoxArt = null;
                        }
                    }
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                if(model.FileJogo != null && model.FileJogo.ContentLength > 0)
                {
                    if(!string.IsNullOrEmpty(model.UrlArquivo))
                    {
                        var fileName = model.UrlArquivo.Split('/')[3];
                        var gamepath = Path.Combine(Server.MapPath("~/Content/Games/"), fileName);
                        if(System.IO.File.Exists(gamepath))
                        {
                            System.IO.File.Delete(gamepath);
                        }
                        else
                        {
                            model.UrlArquivo = null;
                        }
                    }
                    model.UrlArquivo = "/Content/Games/" + model.NomeArquivo;
                }

                var jogoDTO = Mapper.Map<JogoModel, JogoDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarJogo, jogoDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FileJogo != null && model.FileJogo.ContentLength > 0)
                    {
                        var gameFileName = Path.GetFileName(model.FileJogo.FileName);
                        var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), gameFileName);
                        model.FileJogo.SaveAs(gamePath);
                        model.UrlArquivo = "/Content/Games/" + gameFileName;
                    }
                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                        model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarJogo, jogoDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FileJogo != null && model.FileJogo.ContentLength > 0)
                        {
                            var gameFileName = Path.GetFileName(model.FileJogo.FileName);
                            var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), gameFileName);
                            model.FileJogo.SaveAs(gamePath);
                            model.UrlArquivo = "/Content/Games/" + gameFileName;
                        }
                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                            model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void ExcluirJogo(JogoModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if(!string.IsNullOrEmpty(model.UrlArquivo))
                {
                    var gamePath = Path.Combine(Server.MapPath("~/Content/Games/"), model.UrlArquivo);
                    if (System.IO.File.Exists(gamePath))
                    {
                        System.IO.File.Delete(gamePath);
                    }
                }

                if(!string.IsNullOrEmpty(model.UrlBoxArt))
                {
                    var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), model.UrlBoxArt);
                    if (System.IO.File.Exists(boxPath))
                    {
                        System.IO.File.Delete(boxPath);
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirJogo)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }
        #endregion

        #region Módulo Programa
        [NonAction]
        protected List<ProgramaModel> ListarProgramas()
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosProgramas).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<ProgramaModel>>().Result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosProgramas).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<ProgramaModel>>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }

            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroListaMensagem = result.Message;
            }
            return new List<ProgramaModel>();
        }

        [NonAction]
        protected ProgramaModel BuscarProgramaPorId(int Id)
        {
            PreencherHeaders();

            var response = Cliente.GetAsync(url.UrlApi + url.UrlListarProgramaPorId + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<ProgramaModel>().Result;

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RecarregarHeaders();

                response = Cliente.GetAsync(url.UrlApi + url.UrlListarProgramaPorId + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ProgramaModel>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["Unauthorized"] = result.Status;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }

            else
            {
                var result = response.Content.ReadAsAsync<StatusCode>().Result;
                ViewBag.ErroMensagem = result.Message;
                return null;
            }
        }

        [NonAction]
        protected void CadastrarPrograma(ProgramaModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.NomeArquivo))
                {
                    model.UrlArquivo = "/Content/Programas/" + model.NomeArquivo;
                }

                if (!string.IsNullOrEmpty(model.NomeFoto))
                {
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                var programaDTO = Mapper.Map<ProgramaModel, ProgramaDTO>(model);

                var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirPrograma, programaDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                    {
                        var programaFileName = Path.GetFileName(model.FilePrograma.FileName);
                        var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                        model.FilePrograma.SaveAs(progPath);
                    }

                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirPrograma, programaDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                        {
                            var programaFileName = Path.GetFileName(model.FilePrograma.FileName);
                            var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                            model.FilePrograma.SaveAs(progPath);
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }

                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void AtualizarPrograma(ProgramaModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlBoxArt))
                    {
                        var fileName = model.UrlBoxArt.Split('/')[4];
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), fileName);
                        if (System.IO.File.Exists(boxPath))
                        {
                            System.IO.File.Delete(boxPath);
                        }
                        else
                        {
                            model.UrlBoxArt = null;
                        }
                    }
                    model.UrlBoxArt = "/Content/Images/BoxArt/" + model.NomeFoto;
                }

                if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.UrlArquivo))
                    {
                        var programaFileName = model.UrlArquivo.Split('/')[3];
                        var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                        if (System.IO.File.Exists(progPath))
                        {
                            System.IO.File.Delete(progPath);
                        }
                        else
                        {
                            model.UrlArquivo = null;
                        }
                    }
                    model.UrlArquivo = "/Content/Programas/" + model.NomeArquivo;
                }

                var programaDTO = Mapper.Map<ProgramaModel, ProgramaDTO>(model);

                var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarPrograma, programaDTO).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                    {
                        var programaFileName = Path.GetFileName(model.FilePrograma.FileName);
                        var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                        model.FilePrograma.SaveAs(progPath);
                    }

                    if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                    {
                        var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                        var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                        model.FileBoxArt.SaveAs(boxPath);
                    }

                    Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarPrograma, programaDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (model.FilePrograma != null && model.FilePrograma.ContentLength > 0)
                        {
                            var programaFileName = Path.GetFileName(model.FilePrograma.FileName);
                            var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), programaFileName);
                            model.FilePrograma.SaveAs(progPath);
                            model.UrlArquivo = "/Content/Programas/" + programaFileName;
                        }

                        if (model.FileBoxArt != null && model.FileBoxArt.ContentLength > 0)
                        {
                            var boxFileName = Path.GetFileName(model.FileBoxArt.FileName);
                            var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), boxFileName);
                            model.FileBoxArt.SaveAs(boxPath);
                            model.UrlBoxArt = "/Content/Images/BoxArt/" + boxFileName;
                        }

                        Session["Mensagem"] = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }

        [NonAction]
        protected void ExcluirPrograma(ProgramaModel model)
        {
            PreencherHeaders();

            if (model != null)
            {
                var progPath = Path.Combine(Server.MapPath("~/Content/Programas/"), model.UrlArquivo);
                if (System.IO.File.Exists(progPath))
                {
                    System.IO.File.Delete(progPath);
                }
                var boxPath = Path.Combine(Server.MapPath("~/Content/Images/BoxArt/"), model.UrlBoxArt);
                if (System.IO.File.Exists(boxPath))
                {
                    System.IO.File.Delete(boxPath);
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirPrograma)
                };
                var response = Cliente.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RecarregarHeaders();

                    response = Cliente.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["Unauthorized"] = result.Status;
                        Session["ErroMensagem"] = result.Message;
                    }
                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        Session["ErroMensagem"] = result.Message;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }

                else
                {
                    var result = response.Content.ReadAsAsync<StatusCode>().Result;
                    Session["ErroMensagem"] = result.Message;
                }
            }
            else
            {
                Session["ErroMensagem"] = "Parâmetros incorretos!";
            }
        }
        #endregion

        #region Outros
        [NonAction]
        protected ResponseLoginModel ObterToken(LoginModel model)
        {
            var content = new List<KeyValuePair<string, string>>(new[] 
            {
                            new KeyValuePair<string, string>("username", model.UserName),
                            new KeyValuePair<string, string>("password", model.Password),
                            new KeyValuePair<string, string>("grant_type", "password")
            });

            var stringContent = new FormUrlEncodedContent(content);

            using (var responseSecondLogin = Cliente.PostAsync(url.UrlApi + url.UrlToken, stringContent).Result)
            {
                if (responseSecondLogin.IsSuccessStatusCode)
                {
                    var resultLogin = responseSecondLogin.Content.ReadAsAsync<ResponseLoginModel>().Result;

                    return resultLogin;
                }
                else if (responseSecondLogin.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseSecondLogin.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }

                else
                {
                    var result = responseSecondLogin.Content.ReadAsAsync<ResponseErrorLogin>().Result;
                    ViewBag.ErroMensagem = result.error_description;
                }
            }
            return null;
        }

        [NonAction]
        protected ResponseLoginModel RefreshToken()
        {
            var token = GetRefreshToken();

            var content = new List<KeyValuePair<string, string>>(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", token)
            });

            var stringContent = new FormUrlEncodedContent(content);

            using (var responseSecondLogin = Cliente.PostAsync(url.UrlApi + url.UrlToken, stringContent).Result)
            {
                if (responseSecondLogin.IsSuccessStatusCode)
                {
                    var resultLogin = responseSecondLogin.Content.ReadAsAsync<ResponseLoginModel>().Result;

                    Session["newToken"] = resultLogin.access_token;
                    Session["newRefreshToken"] = resultLogin.refresh_token;

                    return resultLogin;
                }
                else if (responseSecondLogin.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseSecondLogin.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Message;
                }

                else
                {
                    var result = responseSecondLogin.Content.ReadAsAsync<ResponseErrorLogin>().Result;
                    ViewBag.ErroMensagem = result.error_description;
                }
            }
            return null;
        }

        [NonAction]
        public InformacoesMaquinaModel GetInformacoes()
        {
            var inf = new InformacoesMaquinaModel();

            var cpuUsage = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total");

            var cpu = cpuUsage.NextValue();
            Thread.Sleep(1000);
            cpu = cpuUsage.NextValue();

            var diskUsage = new System.Diagnostics.PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

            var disk = diskUsage.NextValue();
            Thread.Sleep(1000);
            disk = diskUsage.NextValue();

            var memoryAvailable = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");

            inf.TotalMemoriaDisponível = (memoryAvailable.NextValue()).ToString();

            inf.UsoDisco = disk.ToString("0.00");

            inf.UsoCPU = Convert.ToInt32(cpu).ToString();

            var memory = (GC.GetTotalMemory(false) / 1024 / 1024);

            inf.UsoMemoria = memory.ToString();

            var totalMemoriaInstalada = GetTotalMemoria();

            inf.TotalMemoriaInstalada = totalMemoriaInstalada.ToString();

            inf.Processador = GetProcessorName();

            return inf;

        }

        [NonAction]
        private void PreencherHeaders()
        {
            var token = string.Empty;

            if(Session["newToken"] != null)
            {
                token = Session["newToken"].ToString();
            }
            else
            {
                token = GetToken();
            }
            

            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }
            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        [NonAction]
        private string GetProcessorName()
        {
            var processor = string.Empty;

            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(
                "SELECT * FROM Win32_Processor");

            foreach(System.Management.ManagementObject share in searcher.Get())
            {
                processor = Convert.ToString(share["Name"]);
            }

            return processor;
        }

        [NonAction]
        private long GetTotalMemoria()
        {
            ulong memoria = 0;

            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(
                "SELECT * FROM Win32_PhysicalMemory");

            foreach (System.Management.ManagementObject share in searcher.Get())
            {
                memoria += Convert.ToUInt64(share["Capacity"]);
            }

            return Convert.ToInt64(memoria/(1024 * 1024 * 1024));
        }

        [NonAction]
        private void RecarregarHeaders()
        {
            if (Session["newToken"] != null)
            {
                var refreshToken = Session["newToken"].ToString();
                Cliente.DefaultRequestHeaders.Remove("Authorization");
                Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + refreshToken);
            }
            else
            {
                var modelToken = RefreshToken();
                Cliente.DefaultRequestHeaders.Remove("Authorization");
                Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + modelToken.access_token);
            }
        }

        [NonAction]
        private string GetToken()
        {
            var token = string.Empty;

            foreach (var item in HttpContext.GetOwinContext().Authentication.User.Claims)
            {
                if (item.Type.Contains("AccessToken"))
                {
                    token = item.Value;
                }
            }
            return token;
        }

        [NonAction]
        private string GetRefreshToken()
        {
            var token = string.Empty;

            foreach (var item in HttpContext.GetOwinContext().Authentication.User.Claims)
            {
                if (item.Type.Contains("RefreshToken"))
                {
                    token = item.Value;
                }
            }
            return token;
        }
        #endregion
    }
}