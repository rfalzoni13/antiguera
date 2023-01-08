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
    [RoutePrefix("api/antiguera/jogo")]
    public class JogoController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IJogoServico _jogoServico;

        public JogoController(IJogoServico jogoServico)
        {
            _jogoServico = jogoServico;
        }

        /// <summary>
        /// Listar todos os jogos
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os jogos</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/jogo/ListarTodos
        [HttpGet]
        [Route("ListarTodos")]
        public HttpResponseMessage ListarTodos()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _jogoServico.ListarTodos();

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
        /// Listar jogo pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o jogo através do Id do mesmo</remarks>
        /// <param name="Id">Id do jogo</param>
        /// <returns></returns>
        // GET api/antiguera/admin/jogo/ListarPorId?id={Id}
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
                    var jogo = _jogoServico.BuscarPorId(Id);

                    if (jogo != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, jogo);
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
        /// Inserir jogo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo jogo passando um objeto no body da requisição no método POST</remarks>
        /// <param name="jogoDto">Objeto do jogo</param>
        /// <returns></returns>
        // POST api/antiguera/admin/jogo/Inserir
        [HttpPost]
        [Route("Inserir")]
        public HttpResponseMessage Inserir([FromBody] JogoDTO jogoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    jogoDto.Created = DateTime.Now;

                    jogoDto.Novo = true;

                    _jogoServico.Adicionar(jogoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Jogo inserido com sucesso!");
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
        /// Atualizar jogo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o jogo passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="jogoDto">Objeto do jogo</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/jogo/Atualizar
        [HttpPut]
        [Route("Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] JogoDTO jogoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    jogoDto.Modified = DateTime.Now;

                    jogoDto.Novo = false;

                    _jogoServico.Atualizar(jogoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo atualizado com sucesso!");
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
        /// Excluir jogo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o jogo passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="jogoDto">Objeto do jogo</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/jogo/Excluir
        [HttpDelete]
        [Route("Excluir")]
        public HttpResponseMessage Excluir([FromBody] JogoDTO jogoDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _jogoServico.Apagar(jogoDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo excluído com sucesso!");
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
