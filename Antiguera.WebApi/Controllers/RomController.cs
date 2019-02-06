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
    [RoutePrefix("api/antiguera/admin/rom")]
    public class RomController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IRomAppServico _romAppServico;

        public RomController(IRomAppServico romAppServico)
        {
            _romAppServico = romAppServico;
        }

        /// <summary>
        /// Listar todas as roms
        /// </summary>
        /// <remarks>Listagem de todas as roms</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/rom/listartodasasroms
        [HttpGet]
        [Route("listartodasasroms")]
        public HttpResponseMessage ListarTodasAsRoms()
        {
            logger.Info("ListarTodasAsRoms - Iniciado");
            try
            {
                var retorno = _romAppServico.BuscarTodos();

                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodasAsRoms - Sucesso!");

                    logger.Info("ListarTodasAsRoms - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodasAsRoms - Nenhum registro encontrado!");
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarTodasAsRoms - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodasAsRoms - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarTodasAsRoms - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar rom pelo Id
        /// </summary>
        /// <remarks>Retorna a rom através do Id da mesma</remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/antiguera/admin/rom/listarromsporid
        [HttpGet]
        [Route("listarromsporid")]
        public HttpResponseMessage ListarRomsPorId(int Id)
        {
            logger.Info("ListarRomsPorId - Iniciado");
            try
            {
                if (Id > 0)
                {
                    var rom = _romAppServico.BuscarPorId(Id);

                    if (rom != null)
                    {
                        logger.Info("ListarRomsPorId - Sucesso!");

                        logger.Info("ListarRomsPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, rom);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarRomsPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarRomsPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarRomsPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarRomsPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarRomsPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarRomsPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir rom
        /// </summary>
        /// <remarks>Insere uma nova rom passando um objeto no body da requisição no método POST</remarks>
        /// <param name="romModel"></param>
        /// <returns></returns>
        // POST api/antiguera/admin/rom/inserirrom
        [HttpPost]
        [Route("inserirrom")]
        public HttpResponseMessage InserirRom([FromBody] RomModel romModel)
        {
            logger.Info("InserirRom - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var rom = Mapper.Map<RomModel, Rom>(romModel);

                    _romAppServico.Adicionar(rom);

                    logger.Info("InserirRom - Sucesso!");

                    logger.Info("InserirRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Rom inserida com sucesso!");
                }
                else
                {
                    logger.Warn("InserirRom - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ListarRomsPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("InserirRom - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar rom
        /// </summary>
        /// <remarks>Atualiza a rom passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="romModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/rom/atualizarrom
        [HttpPut]
        [Route("atualizarrom")]
        public HttpResponseMessage AtualizarRom([FromBody] RomModel romModel)
        {
            logger.Info("AtualizarRom - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var rom = Mapper.Map<RomModel, Rom>(romModel);

                    _romAppServico.Atualizar(rom);

                    logger.Info("AtualizarRom - Sucesso!");

                    logger.Info("AtualizarRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Rom atualizada com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarRom - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarRom - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarRom - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir rom
        /// </summary>
        /// <remarks>Exclui a rom passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="romModel"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/rom/excluirrom
        [HttpDelete]
        [Route("excluirrom")]
        public HttpResponseMessage ExcluirRom([FromBody] RomModel romModel)
        {
            logger.Info("ExcluirRom - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var rom = Mapper.Map<RomModel, Rom>(romModel);

                    _romAppServico.Apagar(rom);

                    logger.Info("ExcluirRom - Sucesso!");

                    logger.Info("ExcluirRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Rom excluída com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirRom - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirRom - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirRom - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ExcluirRom - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar roms
        /// </summary>
        /// <remarks>Deleta uma lista de roms passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/rom/apagarroms
        [HttpDelete]
        [Route("apagarroms")]
        public HttpResponseMessage ApagarRoms([FromBody] int[] Ids)
        {
            logger.Info("ApagarRoms - Iniciado");
            try
            {
                if (Ids.Count() > 0)
                {
                    _romAppServico.ApagarRoms(Ids);

                    logger.Info("ApagarRoms - Sucesso!");

                    logger.Info("ApagarRoms - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Rom(s) excluída(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarRoms - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Array preenchido incorretamente!";

                    logger.Info("ApagarRoms - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarRoms - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ApagarRoms - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}