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
    [RoutePrefix("Api/Acesso")]
    public class AcessoController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IAcessoServico _acessoServico;

        public AcessoController(IAcessoServico acessoServico)
        {
            _acessoServico = acessoServico;
        }

        /// <summary>
        /// Listar todos os acessos
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os acessos</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/acesso/ListarTodos
        [HttpGet]
        [Route("ListarTodos")]
        public HttpResponseMessage ListarTodos()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _acessoServico.ListarTodos();

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
        /// Listar acesso pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o acesso através do Id do mesmo</remarks>
        /// <param name="Id">Id do acesso</param>
        /// <returns></returns>
        // GET api/antiguera/admin/acesso/ListarPorId?id={Id}
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
                    var acesso = _acessoServico.BuscarPorId(Id);

                    if (acesso != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, acesso);
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
        /// Inserir acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo acesso passando um objeto no body da requisição no método POST</remarks>
        /// <param name="acessoDto">Objeto do acesso</param>
        /// <returns></returns>
        // POST api/antiguera/admin/acesso/Adicionar
        [HttpPost]
        [Route("Adicionar")]
        public HttpResponseMessage Adicionar([FromBody] AcessoDTO acessoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    acessoDto.Created = DateTime.Now;

                    acessoDto.Novo = true;

                    _acessoServico.Adicionar(acessoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Acesso inserido com sucesso!");
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
        /// Atualizar acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o acesso passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="acessoDto">Objeto do acesso</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/acesso/Atualizar
        [HttpPut]
        [Route("Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] AcessoDTO acessoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    acessoDto.Modified = DateTime.Now;

                    acessoDto.Novo = false;

                    _acessoServico.Atualizar(acessoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Acesso atualizado com sucesso!");
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
        /// Excluir acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o acesso passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="acessoDto">Objeto do acesso</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/acesso/Deletar
        [HttpDelete]
        [Route("Deletar")]
        public HttpResponseMessage Deletar([FromBody] AcessoDTO acessoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _acessoServico.Apagar(acessoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Acesso excluído com sucesso!");
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
