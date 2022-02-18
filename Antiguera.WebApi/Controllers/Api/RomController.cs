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
    [RoutePrefix("api/antiguera/admin/rom")]
    public class RomController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRomServico _romServico;

        public RomController(IRomServico romServico)
        {
            _romServico = romServico;
        }

        /// <summary>
        /// Listar todas as roms
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todas as roms</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/rom/ListarTodasAsRoms
        [HttpGet]
        [Route("ListarTodasAsRoms")]
        public HttpResponseMessage ListarTodasAsRoms()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                var retorno = _romServico.ListarTodos();

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
        /// Listar rom pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna a rom através do Id da mesma</remarks>
        /// <param name="Id">Id da rom</param>
        /// <returns></returns>
        // GET api/antiguera/admin/rom/ListarRomsPorId?id={Id}
        [HttpGet]
        [Route("ListarRomsPorId")]
        public HttpResponseMessage ListarRomsPorId(int Id)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (Id > 0)
                {
                    var rom = _romServico.BuscarPorId(Id);

                    if (rom != null)
                    {
                        _logger.Info(action + " - Sucesso!");

                        _logger.Info(action + " - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, rom);
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
        /// Inserir rom
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere uma nova rom passando um objeto no body da requisição no método POST</remarks>
        /// <param name="romDto">Objeto da rom</param>
        /// <returns></returns>
        // POST api/antiguera/admin/rom/InserirRom
        [HttpPost]
        [Route("InserirRom")]
        public HttpResponseMessage InserirRom([FromBody] RomDTO romDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    romDto.Created = DateTime.Now;

                    romDto.Novo = true;

                    _romServico.Adicionar(romDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.Created, "Rom inserida com sucesso!");
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
        /// Atualizar rom
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza a rom passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="romDto">Objeto da rom</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/rom/AtualizarRom
        [HttpPut]
        [Route("AtualizarRom")]
        public HttpResponseMessage AtualizarRom([FromBody] RomDTO romDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    romDto.Modified = DateTime.Now;

                    romDto.Novo = false;

                    _romServico.Atualizar(romDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Rom atualizada com sucesso!");
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
        /// Excluir rom
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui a rom passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="romDto">Objeto da rom</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/rom/ExcluirRom
        [HttpDelete]
        [Route("ExcluirRom")]
        public HttpResponseMessage ExcluirRom([FromBody] RomDTO romDto)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            _logger.Info(action + " - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    _romServico.Apagar(romDto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");

                    return Request.CreateResponse(HttpStatusCode.OK, "Rom excluída com sucesso!");
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
