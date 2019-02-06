﻿using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Models;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers
{
    [AllowAnonymous]
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin/emulador")]
    public class EmuladorController : ApiController
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
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosEmuladores - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarTodosEmuladores - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar emulador pelo Id
        /// </summary>
        /// <remarks>Retorna o emulador através do Id do mesmo</remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/antiguera/admin/emulador/listaremuladoresporid
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
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarEmuladoresPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarEmuladoresPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarEmuladoresPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarEmuladoresPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";

                logger.Info("ListarEmuladoresPorId - Finalizado");
                stats.Exception = e.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir emulador
        /// </summary>
        /// <remarks>Insere um novo emulador passando um objeto no body da requisição no método POST</remarks>
        /// <param name="emuladorModel"></param>
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
                    var emulador = Mapper.Map<EmuladorModel, Emulador>(emuladorModel);

                    _emuladorAppServico.Adicionar(emulador);

                    logger.Info("InserirEmulador - Sucesso!");

                    logger.Info("InserirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Emulador inserido com sucesso!");
                }
                else
                {
                    logger.Warn("InserirEmulador - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("InserirEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar emulador
        /// </summary>
        /// <remarks>Atualiza o emulador passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="emuladorModel"></param>
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
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir emulador
        /// </summary>
        /// <remarks>Exclui o emulador passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="emuladorModel"></param>
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
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirEmulador - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirEmulador - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ExcluirEmulador - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar emuladores
        /// </summary>
        /// <remarks>Deleta uma lista de emuladores passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids"></param>
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
                    stats.Mensagem = "Array preenchido incorretamente!";

                    logger.Info("ApagarEmuladores - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarEmuladores - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ApagarEmuladores - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}