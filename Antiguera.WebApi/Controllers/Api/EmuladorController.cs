using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Controllers.Api.Base;
using Antiguera.WebApi.Models;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers.Api
{
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin/emulador")]
    public class EmuladorController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IEmuladorAppServico _emuladorAppServico;

        public EmuladorController(IEmuladorAppServico emuladorAppServico)
        {
            _emuladorAppServico = emuladorAppServico;
        }

        /// <summary>
        /// Listar todos os emuladores
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os emuladores</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/emulador/listartodososemuladores
        [HttpGet]
        [Route("listartodososemuladores")]
        public HttpResponseMessage ListarTodosEmuladores()
        {
            logger.Info("ListarTodosEmuladores - Iniciado");
            try
            {
                var retorno = _emuladorAppServico.BuscarTodos();

                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodosEmuladores - Sucesso!");

                    logger.Info("ListarTodosEmuladores - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosEmuladores - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosEmuladores - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ListarTodosEmuladores - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
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
        // GET api/antiguera/admin/emulador/listaremuladoresporid?id={Id}
        [HttpGet]
        [Route("listaremuladoresporid")]
        public HttpResponseMessage ListarEmuladoresPorId(int Id)
        {
            logger.Info("ListarEmuladoresPorId - Iniciado");
            try
            {
                if(Id > 0)
                {
                    var emulador = _emuladorAppServico.BuscarPorId(Id);

                    if (emulador != null)
                    {
                        logger.Info("ListarEmuladoresPorId - Sucesso!");

                        logger.Info("ListarEmuladoresPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, emulador);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarEmuladoresPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Parâmetro incorreto!";

                    logger.Info("ListarEmuladoresPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarEmuladoresPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarEmuladoresPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarEmuladoresPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ListarEmuladoresPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Pesquisar emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Efetua a pesquisa do emulador por qualquer dado inserido
        /// </remarks>
        /// <param name="busca">String de busca</param>
        /// <returns></returns>
        //GET api/antiguera/admin/emulador/pesquisaemulador?busca={busca}
        [HttpGet]
        [Route("pesquisaemulador")]
        public HttpResponseMessage PesquisaEmulador([FromUri] string busca)
        {
            logger.Info("PesquisaEmulador - Iniciar");
            try
            {
                if (!string.IsNullOrEmpty(busca))
                {
                    var isNumeric = int.TryParse(busca, out int n);

                    List<Emulador> retorno = null;

                    if (isNumeric)
                    {
                        retorno = _emuladorAppServico.BuscaQuery(x => x.Id == n).ToList();
                    }

                    else
                    {
                        retorno = _emuladorAppServico.BuscaQuery(x => x.Nome.Contains(busca) ||
                                    x.Nome == busca || RemoveDiacritics(x.Nome).ToLower().Contains(busca.ToLower()) ||
                                    x.Console.Contains(busca) || RemoveDiacritics(x.Console).ToLower().Contains(busca.ToLower())
                                    || RemoveDiacritics(x.Descricao).ToLower().Contains(busca.ToLower())).ToList();
                    }

                    if (retorno != null && retorno.Count > 0)
                    {
                        logger.Info("PesquisaEmulador - Sucesso!");

                        logger.Info("PesquisaEmulador - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, retorno);
                    }

                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("PesquisaEmulador - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("PesquisaEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Warn("PesquisaEmulador - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("PesquisaEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }
            catch (Exception e)
            {
                logger.Error("PesquisaEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("PesquisaEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo emulador passando um objeto no body da requisição no método POST</remarks>
        /// <param name="emuladorModel">Objeto do emulador</param>
        /// <returns></returns>
        // POST api/antiguera/admin/emulador/inseriremulador
        [HttpPost]
        [Route("inseriremulador")]
        public HttpResponseMessage InserirEmulador([FromBody] EmuladorModel emuladorModel)
        {
            logger.Info("InserirEmulador - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    emuladorModel.Created = DateTime.Now;

                    emuladorModel.Novo = true;

                    var emulador = Mapper.Map<EmuladorModel, Emulador>(emuladorModel);

                    _emuladorAppServico.Adicionar(emulador);

                    logger.Info("InserirEmulador - Sucesso!");

                    logger.Info("InserirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.Created, "Emulador inserido com sucesso!");
                }
                else
                {
                    logger.Warn("InserirEmulador - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("InserirEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o emulador passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="emuladorModel">Objeto do emulador</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/emulador/atualizaremulador
        [HttpPut]
        [Route("atualizaremulador")]
        public HttpResponseMessage AtualizarEmulador([FromBody] EmuladorModel emuladorModel)
        {
            logger.Info("AtualizarEmulador - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    emuladorModel.Modified = DateTime.Now;

                    emuladorModel.Novo = false;

                    var emulador = Mapper.Map<EmuladorModel, Emulador>(emuladorModel);

                    _emuladorAppServico.Atualizar(emulador);

                    logger.Info("AtualizarEmulador - Sucesso!");

                    logger.Info("AtualizarEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarEmulador - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("AtualizarEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir emulador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o emulador passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="emuladorModel">Objeto do emulador</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/emulador/excluiremulador
        [HttpDelete]
        [Route("excluiremulador")]
        public HttpResponseMessage ExcluirEmulador([FromBody] EmuladorModel emuladorModel)
        {
            logger.Info("ExcluirEmulador - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var emulador = Mapper.Map<EmuladorModel, Emulador>(emuladorModel);

                    _emuladorAppServico.Apagar(emulador);

                    logger.Info("ExcluirEmulador - Sucesso!");

                    logger.Info("ExcluirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador excluído com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirEmulador - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ExcluirEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar emuladores
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Deleta uma lista de emuladores passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids">Ids de emuladores</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/emulador/apagaremuladores
        [HttpDelete]
        [Route("apagaremuladores")]
        public HttpResponseMessage ApagarEmuladores([FromBody] int[] Ids)
        {
            logger.Info("ApagarEmuladores - Iniciado");
            try
            {
                if(Ids.Count() > 0)
                {
                    _emuladorAppServico.ApagarEmuladores(Ids);

                    logger.Info("ApagarEmuladores - Sucesso!");

                    logger.Info("ApagarEmuladores - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador(es) excluído(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarEmuladores - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Array preenchido incorretamente!";

                    logger.Info("ApagarEmuladores - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarEmuladores - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ApagarEmuladores - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}
