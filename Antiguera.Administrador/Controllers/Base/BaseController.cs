using Antiguera.Administrador.Config;
using Antiguera.Administrador.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public ActionResult _Header()
        {
            HeaderModel model = new HeaderModel();
            model.Usuarios = ListarUsuarios();
            
            model.Jogos = ListarJogos();
            return PartialView(model);
        }
        #endregion

        #region Módulo Usuário
        [NonAction]
        protected List<UsuarioModel> ListarUsuarios()
        {
            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosUsuarios).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<List<UsuarioModel>>().Result;
                }
                else if (responseUsuario.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
                return new List<UsuarioModel>();
            }
        }

        [NonAction]
        protected UsuarioModel BuscarUsuarioPorId(int Id)
        {
            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPorId + Id).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

                }
                else if (responseUsuario.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected UsuarioModel BuscarUsuarioPorLoginOuEmail(string userData)
        {
            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + userData).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

                }
                else if (responseUsuario.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected bool CadastrarUsuario(UsuarioModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarUsuario(UsuarioModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarAdmin(UsuarioModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarAdmin, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarSenhaUsuario(UsuarioModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenhaUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarSenhaAdmin(UsuarioModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenhaAdmin, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool ExcluirUsuario(UsuarioModel model)
        {
            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirUsuario)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Módulo Emulador
        [NonAction]
        protected List<EmuladorModel> ListarEmuladores()
        {
            using (var responseEmulador = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosEmuladores).Result)
            {
                if (responseEmulador.IsSuccessStatusCode)
                {
                    return responseEmulador.Content.ReadAsAsync<List<EmuladorModel>>().Result;
                }
                else if (responseEmulador.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
                return new List<EmuladorModel>();
            }
        }

        [NonAction]
        protected EmuladorModel BuscarEmuladorPorId(int Id)
        {
            using (var responseEmulador = Cliente.GetAsync(url.UrlApi + url.UrlListarEmuladorPorId + Id).Result)
            {
                if (responseEmulador.IsSuccessStatusCode)
                {
                    return responseEmulador.Content.ReadAsAsync<EmuladorModel>().Result;

                }
                else if (responseEmulador.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected bool CadastrarEmulador(EmuladorModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirEmulador, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarEmulador(EmuladorModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarEmulador, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool ExcluirEmulador(EmuladorModel model)
        {
            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirEmulador)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Módulo Rom
        [NonAction]
        protected List<RomModel> ListarRoms()
        {
            using (var responseRom = Cliente.GetAsync(url.UrlApi + url.UrlListarTodasRoms).Result)
            {
                if (responseRom.IsSuccessStatusCode)
                {
                    return responseRom.Content.ReadAsAsync<List<RomModel>>().Result;
                }
                else if (responseRom.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
                return new List<RomModel>();
            }
        }

        [NonAction]
        protected RomModel BuscarRomPorId(int Id)
        {
            using (var responseRom = Cliente.GetAsync(url.UrlApi + url.UrlListarRomPorId + Id).Result)
            {
                if (responseRom.IsSuccessStatusCode)
                {
                    return responseRom.Content.ReadAsAsync<RomModel>().Result;

                }
                else if (responseRom.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected bool CadastrarRom(RomModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirRom, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarRom(RomModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarRom, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool ExcluirRom(RomModel model)
        {
            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirRom)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Módulo Jogo
        [NonAction]
        protected List<JogoModel> ListarJogos()
        {
            using (var responseJogo = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosJogos).Result)
            {
                if (responseJogo.IsSuccessStatusCode)
                {
                    return responseJogo.Content.ReadAsAsync<List<JogoModel>>().Result;
                }
                else if (responseJogo.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
                return new List<JogoModel>();
            }
        }

        [NonAction]
        protected JogoModel BuscarJogoPorId(int Id)
        {
            using (var responseJogo = Cliente.GetAsync(url.UrlApi + url.UrlListarJogoPorId + Id).Result)
            {
                if (responseJogo.IsSuccessStatusCode)
                {
                    return responseJogo.Content.ReadAsAsync<JogoModel>().Result;

                }
                else if (responseJogo.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected bool CadastrarJogo(JogoModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirJogo, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarJogo(JogoModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarJogo, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool ExcluirJogo(JogoModel model)
        {
            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirJogo)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Módulo Programa
        [NonAction]
        protected List<ProgramaModel> ListarProgramas()
        {
            using (var responsePrograma = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosProgramas).Result)
            {
                if (responsePrograma.IsSuccessStatusCode)
                {
                    return responsePrograma.Content.ReadAsAsync<List<ProgramaModel>>().Result;
                }
                else if (responsePrograma.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
                return new List<ProgramaModel>();
            }
        }

        [NonAction]
        protected ProgramaModel BuscarProgramaPorId(int Id)
        {
            using (var responsePrograma = Cliente.GetAsync(url.UrlApi + url.UrlListarProgramaPorId + Id).Result)
            {
                if (responsePrograma.IsSuccessStatusCode)
                {
                    return responsePrograma.Content.ReadAsAsync<ProgramaModel>().Result;

                }
                else if (responsePrograma.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }

        [NonAction]
        protected bool CadastrarPrograma(ProgramaModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirPrograma, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        protected bool AtualizarPrograma(ProgramaModel model)
        {
            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarPrograma, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.ErroMensagem = "Parâmetros incorretos!";
                return false;
            }
        }

        [NonAction]
        protected bool ExcluirPrograma(ProgramaModel model)
        {
            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirPrograma)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Outros
        [NonAction]
        protected void ClearCookies()
        {
            var cookieUser = new HttpCookie("usrDt")
            {
                Expires = DateTime.Now.AddYears(-1)
            };

            var cookieToken = new HttpCookie("tknUs")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(cookieUser);
            Response.Cookies.Add(cookieToken);
        }

        [NonAction]
        protected void SetInfCookies(ResponseLoginModel responseModel, UsuarioModel usuarioModel)
        {
            var cookieUser = new HttpCookie("usrDt")
            {
                Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuarioModel.Id + usuarioModel.Email)),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(1),
                Path = "/Content/Cookies"
            };

            var cookieToken = new HttpCookie("tknUs")
            {
                Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(responseModel.access_token)),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(1),
                Path = "/Content/Cookies"
            };

            Response.Cookies.Add(cookieUser);
            Response.Cookies.Add(cookieToken);
        }
        #endregion
    }
}