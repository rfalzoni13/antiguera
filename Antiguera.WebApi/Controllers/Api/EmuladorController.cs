using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Utils;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers.Api
{
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin/emulador")]
    public class EmuladorController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IEmuladorServico _emuladorServico;

        public EmuladorController(IEmuladorServico emuladorServico)
        {
            _emuladorServico = emuladorServico;
        }

        /// <summary>
        /// Listar todos os emuladores
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os emuladores</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/emulador/ListarTodosEmuladores
        [HttpGet]
        [Route("ListarTodosEmuladores")]
        public HttpResponseMessage ListarTodosEmuladores()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _emuladorServico.ListarTodos();

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
        /// Listar emulador pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o emulador através do Id do mesmo</remarks>
        /// <param name="Id">Id do emulador</param>
        /// <returns></returns>
        // GET api/antiguera/admin/emulador/ListarEmuladoresPorId?id={Id}
        [HttpGet]
        [Route("ListarEmuladoresPorId")]
        public HttpResponseMessage ListarEmuladoresPorId(int Id)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if(Id > 0)
                {
                    var emulador = _emuladorServico.BuscarPorId(Id);

                    if (emulador != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, emulador);
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
                if(ex.Response.StatusCode == HttpStatusCode.NotFound)
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
        /// Inserir emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo emulador passando um objeto no body da requisição no método POST</remarks>
        /// <param name="emuladorDto">Objeto do emulador</param>
        /// <returns></returns>
        // POST api/antiguera/admin/emulador/InserirEmulador
        [HttpPost]
        [Route("InserirEmulador")]
        public HttpResponseMessage InserirEmulador([FromBody] EmuladorDTO emuladorDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    emuladorDto.Created = DateTime.Now;

                    emuladorDto.Novo = true;

                    _emuladorServico.Adicionar(emuladorDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Emulador inserido com sucesso!");
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
        /// Atualizar emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o emulador passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="emuladorDto">Objeto do emulador</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/emulador/AtualizarEmulador
        [HttpPut]
        [Route("AtualizarEmulador")]
        public HttpResponseMessage AtualizarEmulador([FromBody] EmuladorDTO emuladorDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    emuladorDto.Modified = DateTime.Now;

                    emuladorDto.Novo = false;

                    _emuladorServico.Atualizar(emuladorDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador atualizado com sucesso!");
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
        /// Excluir emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o emulador passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="emuladorDto">Objeto do emulador</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/emulador/ExcluirEmulador
        [HttpDelete]
        [Route("ExcluirEmulador")]
        public HttpResponseMessage ExcluirEmulador([FromBody] EmuladorDTO emuladorDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    _emuladorServico.Apagar(emuladorDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador excluído com sucesso!");
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
    }
}
