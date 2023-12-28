using Antiguera.Api.Utils;
using Antiguera.Dominio.DTO;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Dominio.Interfaces.Servicos;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.Api.Controllers.Admin
{
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("Api/Usuario")]
    public class UsuarioController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IAccountServico _accountServico;
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IAccountServico accountServico, IUsuarioServico usuarioServico)
        {
            _accountServico = accountServico;
            _usuarioServico = usuarioServico;
        }

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
                var retorno = _usuarioServico.ListarTodos();

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
                    var usuario = _usuarioServico.ListarPorId(Id);

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

        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="register">Objeto de registro usuário</param>
        /// <returns></returns>
        // POST Api/Usuario/Inserir
        [HttpPost]
        [AllowAnonymous]
        [Route("Inserir")]
        public HttpResponseMessage Inserir([FromBody] ApplicationUserRegisterDTO register)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _accountServico.Adicionar(register);

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
        /// Atualizar usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="register">Objeto de registro do usuário</param>
        /// <returns></returns>
        // PUT Api/Usuario/Atualizar
        [HttpPut]
        [Route("Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] ApplicationUserRegisterDTO register)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _accountServico.Atualizar(register);

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
        /// Excluir usuario
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o usuario passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="register">Objeto de registro do usuario</param>
        /// <returns></returns>
        // DELETE Api/Usuario/Excluir
        [HttpDelete]
        [Route("Excluir")]
        public HttpResponseMessage Excluir([FromBody] ApplicationUserRegisterDTO register)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _accountServico.Apagar(register);

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

        ///// <summary>
        ///// Atualizar senha do usuário
        ///// </summary>
        ///// <response code="400">Bad Request</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Server Error</response>
        ///// <remarks>Atualiza a senha do usuário passando o objeto no body da requisição pelo método PUT</remarks>
        ///// <param name="usuarioDto">Objeto do usuário</param>
        ///// <returns></returns>
        // PUT Api/Usuario/AtualizarSenha
        //[HttpPut]
        //[Route("AtualizarSenha")]
        //public async Task<HttpResponseMessage> AtualizarSenha([FromBody]UsuarioDTO usuarioDto)
        //{
        //    string action = this.ActionContext.ActionDescriptor.ActionName;;
        //    _logger.Info(action + " - Iniciado");
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //                await UserManager.RemovePasswordAsync(user.Id);

        //                await UserManager.AddPasswordAsync(user.Id, usuarioDto.Senha);

        //                _logger.Info(action + " - Sucesso!");

        //                _logger.Info(action + " - Finalizado");

        //                GravarHistorico(usuarioDto.Id, ETipoHistorico.AtualizarSenha);

        //                return Request.CreateResponse(HttpStatusCode.OK, "Senha alterada com sucesso!");


        //        else
        //        {
        //            return RetornoRequisicaoInvalida(action, "Por favor, preencha os campos corretamente!");
        //        }
        //    }

        //    catch (HttpResponseException ex)
        //    {
        //        return RetornoExceptionNaoEncontrado(ex, action, "Nenhum registro encontrado!");
        //    }

        //    catch (Exception ex)
        //    {
        //        return RetornoExceptionErroInterno(ex, action);
        //    }
        //}
    }
}
