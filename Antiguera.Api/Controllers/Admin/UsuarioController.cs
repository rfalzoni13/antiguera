using Antiguera.Api.Models;
using Antiguera.Api.Utils;
using Antiguera.Servicos.Servicos.Identity;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Antiguera.Api.Controllers.Admin
{
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("Api/Usuario")]
    public class UsuarioController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly UsuarioServico _usuarioServico;

        public UsuarioController(UsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        #region Pesquisas
        /// <summary>
        /// Listar todos os usuarios
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os usuarios</remarks>
        /// <returns></returns>
        // GET Api/Usuario/ListarTodos
        [HttpGet]
        [Route("ListarTodos")]
        public HttpResponseMessage ListarTodos()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _usuarioServico.ListarTodosUsuarios();

                if (retorno != null && retorno.Count() > 0)
                {
                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ResponseMessageHelper.RetornoExceptionNaoEncontrado(ex, Request, _logger, action, "Nenhum registro encontrado!");
                }

                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }

            catch (Exception ex)
            {
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Listar usuário pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o usuário através do Id do mesmo</remarks>
        /// <param name="Id">Id do usuário</param>
        /// <returns></returns>
        // GET Api/Usuario/ListarPorId?id={Id}
        [HttpGet]
        [AllowAnonymous]
        [Route("ListarPorId")]
        public HttpResponseMessage ListarPorId(Guid Id)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (Id != null)
                {
                    var usuario = _usuarioServico.ListarUsuarioPorId(Id);

                    if (usuario != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, usuario);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    return ResponseMessageHelper.RetornoRequisicaoInvalida(Request, _logger, action, "Parâmetro incorreto!");
                }
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ResponseMessageHelper.RetornoExceptionNaoEncontrado(ex, Request, _logger, action, "Nenhum registro encontrado!");
                }

                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }

            catch (Exception ex)
            {
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region Inserir Usuário
        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro usuário</param>
        /// <returns></returns>
        // POST Api/Usuario/Inserir
        [HttpPost]
        [AllowAnonymous]
        [Route("Inserir")]
        public HttpResponseMessage Inserir([FromBody] ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                    _usuarioServico.Adicionar(userDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Usuário incluído com sucesso!");
                }
                else
                {
                    return ResponseMessageHelper.RetornoRequisicaoInvalida(Request, _logger, action, "Por favor, preencha os campos corretamente!");
                }
            }

            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ResponseMessageHelper.RetornoExceptionNaoEncontrado(ex, Request, _logger, action, "Nenhum registro encontrado!");
                }

                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }

            catch (Exception ex)
            {
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Inserir usuário modo assíncrono
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST de forma assíncrona</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro usuário</param>
        /// <returns></returns>

        // POST: /Usuario/InserirAsync
        [HttpPost]
        [Route("InserirAsync")]
        public async Task<HttpResponseMessage> InserirAsync(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");

                var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                await _usuarioServico.AdicionarAsnyc(userDto);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region Atualizar Usuário
        /// <summary>
        /// Atualizar usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro do usuário</param>
        /// <returns></returns>
        // PUT Api/Usuario/Atualizar
        [HttpPut]
        [Route("Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                    _usuarioServico.Atualizar(userDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário atualizado com sucesso!");
                }
                else
                {
                    return ResponseMessageHelper.RetornoRequisicaoInvalida(Request, _logger, action, "Por favor, preencha os campos corretamente!");
                }
            }

            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ResponseMessageHelper.RetornoExceptionNaoEncontrado(ex, Request, _logger, action, "Nenhum registro encontrado!");
                }

                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }

            catch (Exception ex)
            {
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Atualizar usuário modo assíncrono
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT de forma assíncrona</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro do usuário</param>
        // PUT: /Usuario/AtualizarAsync
        [HttpPut]
        [Authorize]
        [Route("AtualizarAsync")]
        public async Task<HttpResponseMessage> AtualizarAsync(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");

                var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                await _usuarioServico.AtualizarAsync(userDto);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region Excluir Usuário
        /// <summary>
        /// Excluir usuario
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o usuario passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro do usuario</param>
        /// <returns></returns>
        // DELETE Api/Usuario/Excluir
        [HttpDelete]
        [Route("Excluir")]
        public HttpResponseMessage Excluir([FromBody] ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                    _usuarioServico.Apagar(userDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Usuario excluído com sucesso!");
                }
                else
                {
                    return ResponseMessageHelper.RetornoRequisicaoInvalida(Request, _logger, action, "Por favor, preencha os campos corretamente!");
                }
            }

            catch (Exception ex)
            {
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Excluir usuario
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o usuario passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="applicationUserRegisterModel">Objeto de registro do usuario</param>
        /// <returns></returns>
        //DELETE: /Usuario/ExcluirAsync
        [HttpDelete]
        [CustomAuthorize]
        [Route("ExcluirAsync")]
        public async Task<HttpResponseMessage> ExcluirAsync(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");

                var userDto = ApplicationUserRegisterModel.ConvertToDTO(applicationUserRegisterModel);

                await _usuarioServico.ApagarAsync(userDto);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário deletado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion
    }
}
