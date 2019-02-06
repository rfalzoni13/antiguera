using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Models;
using AutoMapper;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AntigueraWebApi.Controllers
{
    [AllowAnonymous]
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin/jogo")]
    public class JogoController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IJogoAppServico _jogoAppServico;

        public JogoController(IJogoAppServico jogoAppServico)
        {
            _jogoAppServico = jogoAppServico;
        }

        /// <summary>
        /// Listar todos os jogos
        /// </summary>
        /// <remarks>Listagem de todos os jogos</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/jogo/listartodososjogos
        [HttpGet]
        [Route("listartodososjogos")]
        public HttpResponseMessage ListarTodosJogos()
        {
            logger.Info("ListarTodosJogos - Iniciado");
            try
            {
                var retorno = _jogoAppServico.BuscarTodos();

                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodosJogos - Sucesso!");

                    logger.Info("ListarTodosJogos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosJogos - Error: " + e);
                stats.Status = e.Response.StatusCode;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarTodosJogos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosJogos - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarTodosJogos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar jogo pelo Id
        /// </summary>
        /// <remarks>Retorna o jogo através do Id do mesmo</remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/antiguera/admin/jogo/listarjogosporid
        [HttpGet]
        [Route("listarjogosporid")]
        public HttpResponseMessage ListarJogosPorId(int Id)
        {
            logger.Info("ListarJogosPorId - Iniciado");
            try
            {
                if(Id > 0)
                {
                    var jogo = _jogoAppServico.BuscarPorId(Id);

                    if (jogo != null)
                    {
                        logger.Info("ListarJogosPorId - Sucesso!");

                        logger.Info("ListarJogosPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, jogo);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarJogosPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarJogosPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarJogosPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarJogosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarJogosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarJogosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir jogo
        /// </summary>
        /// <remarks>Insere um novo jogo passando um objeto no body da requisição no método POST</remarks>
        /// <param name="jogoModel"></param>
        /// <returns></returns>
        // POST api/antiguera/admin/jogo/inserirjogo
        [HttpPost]
        [Route("inserirjogo")]
        public HttpResponseMessage InserirJogo([FromBody] JogoModel jogoModel)
        {
            logger.Info("InserirJogo - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var jogo = Mapper.Map<JogoModel, Jogo>(jogoModel);

                    _jogoAppServico.Adicionar(jogo);

                    logger.Info("InserirJogo - Sucesso!");

                    logger.Info("InserirJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo inserido com sucesso!");
                }
                else
                {
                    logger.Warn("InserirJogo - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirJogo - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("InserirJogo - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar jogo
        /// </summary>
        /// <remarks>Atualiza o jogo passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="jogoModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/jogo/atualizarjogo
        [HttpPut]
        [Route("atualizarjogo")]
        public HttpResponseMessage AtualizarJogo([FromBody] JogoModel jogoModel)
        {
            logger.Info("AtualizarJogo - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var jogo = Mapper.Map<JogoModel, Jogo>(jogoModel);

                    _jogoAppServico.Atualizar(jogo);

                    logger.Info("AtualizarJogo - Sucesso!");

                    logger.Info("AtualizarJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarJogo - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarJogo - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarJogo - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir jogo
        /// </summary>
        /// <remarks>Exclui o jogo passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="jogoModel"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/jogo/excluirjogo
        [HttpDelete]
        [Route("excluirjogo")]
        public HttpResponseMessage ExcluirJogo([FromBody] JogoModel jogoModel)
        {
            logger.Info("ExcluirJogo - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var jogo = Mapper.Map<JogoModel, Jogo>(jogoModel);

                    _jogoAppServico.Apagar(jogo);

                    logger.Info("ExcluirJogo - Sucesso!");

                    logger.Info("ExcluirJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo excluído com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirJogo - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirJogo - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirJogo - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ExcluirJogo - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar jogos
        /// </summary>
        /// <remarks>Deleta uma lista de jogos passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/jogo/apagarjogos
        [HttpDelete]
        [Route("apagarjogos")]
        public HttpResponseMessage ApagarJogos([FromBody] int[] Ids)
        {
            logger.Info("ApagarJogos - Iniciado");
            try
            {
                if(Ids.Count() > 0)
                {
                    _jogoAppServico.ApagarJogos(Ids);

                    logger.Info("ApagarJogos - Sucesso!");

                    logger.Info("ApagarJogos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Jogo(s) excluído(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarJogos - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Array preenchido incorretamente!";

                    logger.Info("ApagarJogos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }   
            }

            catch (Exception e)
            {
                logger.Error("ApagarJogos - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ApagarJogos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}
