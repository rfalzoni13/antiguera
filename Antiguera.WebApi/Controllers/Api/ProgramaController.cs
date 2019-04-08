﻿using Antiguera.Aplicacao.Interfaces;
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
    [RoutePrefix("api/antiguera/admin/programa")]
    public class ProgramaController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IProgramaAppServico _programaAppServico;

        public ProgramaController(IProgramaAppServico programaAppServico)
        {
            _programaAppServico = programaAppServico;
        }

        /// <summary>
        /// Listar todos os programas
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os programas</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/programa/listartodososprogramas
        [HttpGet]
        [Route("listartodososprogramas")]
        public HttpResponseMessage ListarTodosProgramas()
        {
            logger.Info("ListarTodosProgramas - Iniciado");
            try
            {
                var retorno = _programaAppServico.BuscarTodos();
            
                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodosProgramas - Sucesso!");

                    logger.Info("ListarTodosProgramas - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosProgramas - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarTodosProgramas - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosProgramas - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;
                logger.Info("ListarTodosProgramas - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
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
        // GET api/antiguera/admin/programa/listarprogramasporid?id={Id}
        [HttpGet]
        [Route("listarprogramasporid")]
        public HttpResponseMessage ListarProgramasPorId(int Id)
        {
            logger.Info("ListarProgramasPorId - Iniciado");
            try
            {
                if(Id > 0)
                {
                    var programa = _programaAppServico.BuscarPorId(Id);

                    if (programa != null)
                    {
                        logger.Info("ListarProgramasPorId - Sucesso!");

                        logger.Info("ListarProgramasPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, programa);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarProgramasPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Parâmetro incorreto!";

                    logger.Info("ListarProgramasPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
                
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarProgramasPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarProgramasPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarProgramasPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ListarProgramasPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Pesquisar programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Efetua a pesquisa do programa por qualquer dado inserido
        /// </remarks>
        /// <param name="busca">String de busca</param>
        /// <returns></returns>
        //GET api/antiguera/admin/programa/pesquisaprograma?busca={busca}
        [HttpGet]
        [Route("pesquisaprograma")]
        public HttpResponseMessage PesquisaPrograma([FromUri] string busca)
        {
            logger.Info("PesquisaPrograma - Iniciar");
            try
            {
                if (!string.IsNullOrEmpty(busca))
                {
                    var isNumeric = int.TryParse(busca, out int n);

                    List<Programa> retorno = null;

                    if (isNumeric)
                    {
                        retorno = _programaAppServico.BuscaQuery(x => x.Id == n).ToList();
                    }

                    else
                    {
                        retorno = _programaAppServico.BuscaQuery(x => x.Nome.Contains(busca) ||
                                    x.Nome == busca || RemoveDiacritics(x.Nome).ToLower().Contains(busca.ToLower()) ||
                                    RemoveDiacritics(x.Developer).ToLower().Contains(busca.ToLower())
                                    || RemoveDiacritics(x.Descricao).ToLower().Contains(busca.ToLower()) ||
                                    RemoveDiacritics(x.Publisher).ToLower().Contains(busca.ToLower()) ||
                                    RemoveDiacritics(x.TipoPrograma).ToLower().Contains(busca.ToLower())).ToList();
                    }

                    if (retorno != null && retorno.Count > 0)
                    {
                        logger.Info("PesquisaPrograma - Sucesso!");

                        logger.Info("PesquisaPrograma - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, retorno);
                    }

                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("PesquisaPrograma - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("PesquisaPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Warn("PesquisaPrograma - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("PesquisaPrograma - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }
            catch (Exception e)
            {
                logger.Error("PesquisaPrograma - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("PesquisaPrograma - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir Programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo programa passando um objeto no body da requisição no método POST</remarks>
        /// <param name="programaModel">Objeto do programa</param>
        /// <returns></returns>
        // POST api/antiguera/admin/programa/inserirprograma
        [HttpPost]
        [Route("inserirprograma")]
        public HttpResponseMessage InserirPrograma([FromBody] ProgramaModel programaModel)
        {
            logger.Info("InserirPrograma - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    programaModel.Created = DateTime.Now;

                    programaModel.Novo = true;

                    var programa = Mapper.Map<ProgramaModel, Programa>(programaModel);

                    _programaAppServico.Adicionar(programa);

                    logger.Info("InserirPrograma - Sucesso!");

                    logger.Info("InserirPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.Created, "Programa inserido com sucesso!");
                }
                else
                {
                    logger.Warn("InserirPrograma - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirPrograma - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("InserirPrograma - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o programa passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="programaModel">Objeto do programa</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/programa/atualizarprograma
        [HttpPut]
        [Route("atualizarprograma")]
        public HttpResponseMessage AtualizarPrograma([FromBody] ProgramaModel programaModel)
        {
            logger.Info("AtualizarPrograma - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    programaModel.Modified = DateTime.Now;

                    programaModel.Novo = false;

                    var programa = Mapper.Map<ProgramaModel, Programa>(programaModel);

                    _programaAppServico.Atualizar(programa);

                    logger.Info("AtualizarPrograma - Sucesso!");

                    logger.Info("AtualizarPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Programa atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarPrograma - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarPrograma - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("AtualizarPrograma - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir programa
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o programa passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="programaModel">Objeto do programa</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/programa/excluirprograma
        [HttpDelete]
        [Route("excluirprograma")]
        public HttpResponseMessage ExcluirPrograma([FromBody] ProgramaModel programaModel)
        {
            logger.Info("ExcluirPrograma - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var programa = Mapper.Map<ProgramaModel, Programa>(programaModel);

                    _programaAppServico.Apagar(programa);

                    logger.Info("ExcluirPrograma - Sucesso!");

                    logger.Info("ExcluirPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Programa excluído com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirPrograma - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirPrograma - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirPrograma - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ExcluirPrograma - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar programas
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Deleta uma lista de programas passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids">Ids de programas</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/programa/apagarprogramas
        [HttpDelete]
        [Route("apagarprogramas")]
        public HttpResponseMessage ApagarProgramas([FromBody] int[] Ids)
        {
            logger.Info("ApagarProgramas - Iniciado");
            try
            {
                if(Ids.Count() > 0)
                {
                    _programaAppServico.ApagarProgramas(Ids);

                    logger.Info("ApagarProgramas - Sucesso!");

                    logger.Info("ApagarProgramas - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Programa(s) excluído(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarProgramas - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Array preenchido incorretamente!";

                    logger.Info("ApagarProgramas - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarProgramas - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ApagarProgramas - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}