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
        [Authorize]
        public ActionResult _Header()
        {
            HeaderModel model = new HeaderModel();

            var token = PegarTokenAtual();
            if(!string.IsNullOrEmpty(token))
            {
                model.Usuarios = ListarUsuarios(token);

                model.Jogos = ListarJogos(token);
            }
            else
            {
                token = PegarTokenRefreshAtual();
                if(!string.IsNullOrEmpty(token))
                {
                    model.Usuarios = ListarUsuarios(token);

                    model.Jogos = ListarJogos(token);
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Módulo Usuário
        [NonAction]
        protected List<UsuarioModel> ListarUsuarios(string token)
        {
            if(Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }
            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosUsuarios).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<List<UsuarioModel>>().Result;
                }
                else if (responseUsuario.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
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
            }
            return new List<UsuarioModel>();
        }

        [NonAction]
        protected UsuarioModel BuscarUsuarioPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPorId + Id).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

                }
                else if (responseUsuario.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected UsuarioModel BuscarUsuarioPorLoginOuEmail(string userData, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseUsuario = Cliente.GetAsync(url.UrlApi + url.UrlListarUsuariosPeloLoginOuEmail + userData).Result)
            {
                if (responseUsuario.IsSuccessStatusCode)
                {
                    return responseUsuario.Content.ReadAsAsync<UsuarioModel>().Result;

                }
                else if (responseUsuario.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseUsuario.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected bool CadastrarUsuario(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarUsuario(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarAdmin(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarAdmin, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarSenhaUsuario(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenhaUsuario, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }

                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarSenhaAdmin(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarSenhaAdmin, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirUsuario(UsuarioModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    }

                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
            }
        }
        #endregion

        #region Módulo Permissões
        [NonAction]
        protected List<AcessoModel> ListarAcessos(string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }
            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseAcesso = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosAcessos).Result)
            {
                if (responseAcesso.IsSuccessStatusCode)
                {
                    return responseAcesso.Content.ReadAsAsync<List<AcessoModel>>().Result;
                }
                else if (responseAcesso.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                }
                else if (responseAcesso.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                }

                else
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroListaMensagem = result.Mensagem;
                }
            }
            return new List<AcessoModel>();
        }

        [NonAction]
        protected AcessoModel BuscarAcessoPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseAcesso = Cliente.GetAsync(url.UrlApi + url.UrlListarAcessoPorId + Id).Result)
            {
                if (responseAcesso.IsSuccessStatusCode)
                {
                    return responseAcesso.Content.ReadAsAsync<AcessoModel>().Result;

                }
                else if (responseAcesso.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
                }
                else if (responseAcesso.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }

                else
                {
                    var result = responseAcesso.Content.ReadAsAsync<StatusCode>().Result;
                    ViewBag.ErroMensagem = result.Mensagem;
                    return null;
                }
            }
        }
        
        [NonAction]
        protected bool CadastrarAcesso(AcessoModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirAcesso, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirAcesso(AcessoModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url.UrlApi + url.UrlExcluirAcesso)
                };
                using (var response = Cliente.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                    }

                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
            }
        }
        #endregion

        #region Módulo Emulador
        [NonAction]
        protected List<EmuladorModel> ListarEmuladores(string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseEmulador = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosEmuladores).Result)
            {
                if (responseEmulador.IsSuccessStatusCode)
                {
                    return responseEmulador.Content.ReadAsAsync<List<EmuladorModel>>().Result;
                }
                else if (responseEmulador.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
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
        protected EmuladorModel BuscarEmuladorPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            
            using (var responseEmulador = Cliente.GetAsync(url.UrlApi + url.UrlListarEmuladorPorId + Id).Result)
            {
                if (responseEmulador.IsSuccessStatusCode)
                {
                    return responseEmulador.Content.ReadAsAsync<EmuladorModel>().Result;

                }
                else if (responseEmulador.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseEmulador.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected bool CadastrarEmulador(EmuladorModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirEmulador, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarEmulador(EmuladorModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarEmulador, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirEmulador(EmuladorModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
            }
        }
        #endregion

        #region Módulo Rom
        [NonAction]
        protected List<RomModel> ListarRoms(string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseRom = Cliente.GetAsync(url.UrlApi + url.UrlListarTodasRoms).Result)
            {
                if (responseRom.IsSuccessStatusCode)
                {
                    return responseRom.Content.ReadAsAsync<List<RomModel>>().Result;
                }
                else if (responseRom.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
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
        protected RomModel BuscarRomPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseRom = Cliente.GetAsync(url.UrlApi + url.UrlListarRomPorId + Id).Result)
            {
                if (responseRom.IsSuccessStatusCode)
                {
                    return responseRom.Content.ReadAsAsync<RomModel>().Result;

                }
                else if (responseRom.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseRom.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected bool CadastrarRom(RomModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
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
        protected bool AtualizarRom(RomModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarRom, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirRom(RomModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }
            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
            }
        }
        #endregion

        #region Módulo Jogo
        [NonAction]
        protected List<JogoModel> ListarJogos(string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseJogo = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosJogos).Result)
            {
                if (responseJogo.IsSuccessStatusCode)
                {
                    return responseJogo.Content.ReadAsAsync<List<JogoModel>>().Result;
                }
                else if (responseJogo.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
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
        protected JogoModel BuscarJogoPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responseJogo = Cliente.GetAsync(url.UrlApi + url.UrlListarJogoPorId + Id).Result)
            {
                if (responseJogo.IsSuccessStatusCode)
                {
                    return responseJogo.Content.ReadAsAsync<JogoModel>().Result;

                }
                else if (responseJogo.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responseJogo.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected bool CadastrarJogo(JogoModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirJogo, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarJogo(JogoModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarJogo, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirJogo(JogoModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
            }
        }
        #endregion

        #region Módulo Programa
        [NonAction]
        protected List<ProgramaModel> ListarProgramas(string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responsePrograma = Cliente.GetAsync(url.UrlApi + url.UrlListarTodosProgramas).Result)
            {
                if (responsePrograma.IsSuccessStatusCode)
                {
                    return responsePrograma.Content.ReadAsAsync<List<ProgramaModel>>().Result;
                }
                else if (responsePrograma.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
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
        protected ProgramaModel BuscarProgramaPorId(int Id, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using (var responsePrograma = Cliente.GetAsync(url.UrlApi + url.UrlListarProgramaPorId + Id).Result)
            {
                if (responsePrograma.IsSuccessStatusCode)
                {
                    return responsePrograma.Content.ReadAsAsync<ProgramaModel>().Result;

                }
                else if (responsePrograma.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = responsePrograma.Content.ReadAsAsync<StatusCode>().Result;
                    TempData["Unauthorized"] = result.Status;
                    TempData["ErroMensagem"] = result.Mensagem;
                    return null;
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
        protected bool CadastrarPrograma(ProgramaModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PostAsJsonAsync(url.UrlApi + url.UrlInserirPrograma, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected bool AtualizarPrograma(ProgramaModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (model != null)
            {
                using (var response = Cliente.PutAsJsonAsync(url.UrlApi + url.UrlAtualizarPrograma, model).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Mensagem = response.Content.ReadAsAsync<string>().Result;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                        return false;
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
        protected void ExcluirPrograma(ProgramaModel model, string token)
        {
            if (Cliente.DefaultRequestHeaders.Contains("Authorization"))
            {
                Cliente.DefaultRequestHeaders.Remove("Authorization");
            }

            Cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        TempData["Unauthorized"] = result.Status;
                        TempData["ErroMensagem"] = result.Mensagem;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }

                    else
                    {
                        var result = response.Content.ReadAsAsync<StatusCode>().Result;
                        ViewBag.ErroMensagem = result.Mensagem;
                    }
                }
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
                    ViewBag.ErroMensagem = result.Mensagem;
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
        protected ResponseLoginModel RefreshToken(string token)
        {
            var content = new List<KeyValuePair<string, string>>(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("refresh_token", token)
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
                    ViewBag.ErroMensagem = result.Mensagem;
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
        protected string PegarTokenAtual()
        {
            string token = string.Empty;

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
        protected string PegarTokenRefreshAtual()
        {
            string token = string.Empty;

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