﻿using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Models;
using AutoMapper;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AntigueraWebApi.Controllers
{
    [CustomAuthorize(Roles = "Usuário")]
    [RoutePrefix("api/antiguera/usuario")]
    public class UsuarioController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private IUsuarioAppServico _usuarioAppServico;

        public UsuarioController(IUsuarioAppServico usuarioAppServico)
        {
            _usuarioAppServico = usuarioAppServico;
        }

        /// <summary>
        /// Listar usuário pelo Id
        /// </summary>
        /// <remarks>Retorna o usuário através do Id do mesmo</remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/antiguera/usuario/listarusuariosporid
        [HttpGet]
        [Route("listarusuariosporid")]
        public HttpResponseMessage ListarUsuariosPorId(int Id)
        {
            logger.Info("ListarUsuariosPorId - Iniciado");
            try
            {
                if (Id > 0)
                {
                    var usuario = _usuarioAppServico.BuscarPorId(Id);

                    if (usuario != null)
                    {
                        logger.Info("ListarUsuariosPorId - Sucesso!");

                        logger.Info("ListarUsuariosPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, usuario);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarUsuariosPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarUsuariosPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar usuário pelo Login ou Email
        /// </summary>
        /// <remarks>Efetua a busca do usuário pelo Login ou Email</remarks>
        /// <param name="userData"></param>
        /// <returns></returns>
        // GET api/antiguera/usuario/listarusuariosporloginouemail
        [HttpGet]
        [Route("listarusuariosporloginouemail")]
        public HttpResponseMessage ListarUsuariosPorLoginOuEmail(string userData)
        {
            logger.Info("ListarUsuariosPorLoginOuEmail - Iniciado");
            try
            {
                if (!string.IsNullOrEmpty(userData))
                {
                    var usuario = _usuarioAppServico.BuscarUsuarioPorLoginOuEmail(userData);

                    if (usuario != null)
                    {
                        logger.Info("ListarUsuariosPorLoginOuEmail - Sucesso!");

                        logger.Info("ListarUsuariosPorLoginOuEmail - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, usuario);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarUsuariosPorLoginOuEmail - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarUsuariosPorLoginOuEmail - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.NotFound, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Error("ListarUsuariosPorLoginOuEmail - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("ListarUsuariosPorLoginOuEmail - Finalizado");
                stats.Exception = e.Message;
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";

                logger.Info("ListarUsuariosPorId - Finalizado");
                stats.Exception = e.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // POST api/antiguera/usuario/inserirusuario
        [HttpPost]
        [Route("inserirusuario")]
        public HttpResponseMessage InserirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("InserirUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var senha = BCrypt.HashPassword(usuarioModel.Senha, BCrypt.GenerateSalt());

                    usuarioModel.Senha = senha;

                    var usuario = Mapper.Map<UsuarioModel, Usuario>(usuarioModel);

                    _usuarioAppServico.Adicionar(usuario);

                    logger.Info("InserirUsuario - Sucesso!");

                    logger.Info("InserirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário incluído com sucesso!");
                }
                else
                {
                    logger.Warn("InserirUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("InserirUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar usuário
        /// </summary>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/usuario/atualizarusuario
        [HttpPut]
        [Route("atualizarusuario")]
        public HttpResponseMessage AtualizarUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = Mapper.Map<UsuarioModel, Usuario>(usuarioModel);

                    _usuarioAppServico.Atualizar(usuario);

                    logger.Info("AtualizarUsuario - Sucesso!");

                    logger.Info("AtualizarUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar senha do usuário
        /// </summary>
        /// <remarks>Atualiza a senha do usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/usuario/atualizarsenhausuario
        [HttpPut]
        [Route("atualizarsenhausuario")]
        public HttpResponseMessage AtualizarSenhaUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarSenhaUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var senha = BCrypt.HashPassword(usuarioModel.Senha, BCrypt.GenerateSalt());

                    usuarioModel.Senha = senha;

                    _usuarioAppServico.AlterarSenha(usuarioModel.Id, usuarioModel.Senha);

                    logger.Info("AtualizarSenhaUsuario - Sucesso!");

                    logger.Info("AtualizarSenhaUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Senha alterada com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarSenhaUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarSenhaUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.NotFound, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarSenhaUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarSenhaUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}