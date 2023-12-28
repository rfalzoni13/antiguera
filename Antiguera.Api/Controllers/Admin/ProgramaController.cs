using Antiguera.Api.Utils;
using Antiguera.Dominio.DTO;
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
    [RoutePrefix("Api/Programa")]
    public class ProgramaController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IProgramaServico _programaServico;

        public ProgramaController(IProgramaServico programaServico)
        {
            _programaServico = programaServico;
        }

        /// <summary>
        /// Listar todos os programas
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os programas</remarks>
        /// <returns></returns>
        // GET Api/Programa/ListarTodos
        [HttpGet]
        [Route("ListarTodos")]
        public HttpResponseMessage ListarTodos()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _programaServico.ListarTodos();

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
        /// Listar programa pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o programa através do Id do mesmo</remarks>
        /// <param name="Id">Id do programa</param>
        /// <returns></returns>
        // GET Api/Programa/ListarPorId?id={Id}
        [HttpGet]
        [Route("ListarPorId")]
        public HttpResponseMessage ListarPorId(Guid Id)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (Id != null)
                {
                    var programa = _programaServico.BuscarPorId(Id);

                    if (programa != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, programa);
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
        /// Inserir Programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo programa passando um objeto no body da requisição no método POST</remarks>
        /// <param name="programaDto">Objeto do programa</param>
        /// <returns></returns>
        // POST Api/Programa/Inserir
        [HttpPost]
        [Route("Inserir")]
        public HttpResponseMessage Inserir([FromBody] ProgramaDTO programaDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    programaDto.Created = DateTime.Now;

                    programaDto.Novo = true;

                    _programaServico.Adicionar(programaDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Programa inserido com sucesso!");
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
        /// Atualizar programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o programa passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="programaDto">Objeto do programa</param>
        /// <returns></returns>
        // PUT Api/Programa/Atualizar
        [HttpPut]
        [Route("Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] ProgramaDTO programaDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    programaDto.Modified = DateTime.Now;

                    programaDto.Novo = false;

                    _programaServico.Atualizar(programaDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Programa atualizado com sucesso!");
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
        /// Excluir programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o programa passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="programaDto">Objeto do programa</param>
        /// <returns></returns>
        // DELETE Api/Programa/Excluir
        [HttpDelete]
        [Route("Excluir")]
        public HttpResponseMessage Excluir([FromBody] ProgramaDTO programaDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _programaServico.Apagar(programaDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Programa excluído com sucesso!");
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
