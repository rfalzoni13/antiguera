using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Models;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AntigueraWebApi.Controllers
{
    [AllowAnonymous]
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin")]
    public class AdminController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IUsuarioAppServico _usuarioAppServico;

        public AdminController(IUsuarioAppServico usuarioAppServico)
        {
            _usuarioAppServico = usuarioAppServico;
        }

        /// <summary>
        /// Login no Admin
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Login Admin através da base identity passando no body o objeto do usuário</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/antiguera/admin/loginadmin
        [HttpPost]
        [Route("loginadmin")]
        public async Task<HttpResponseMessage> LoginAdmin([FromBody] LoginModel model)
        {
            logger.Info("LoginAdmin - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    var user = await userManager.FindAsync(model.UserName, model.Password);

                    if(user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        logger.Error("LoginAdmin - Login ou senha incorretos!");
                        stats.Status = HttpStatusCode.BadRequest;
                        stats.Mensagem = "Login ou senha incorretos!";

                        logger.Info("LoginAdmin - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                    }
                }
                else
                {
                    logger.Warn("LoginAdmin - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("LoginAdmin - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("LoginAdmin - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("LoginAdmin - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar todos os usuários
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os usuários</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/listartodososusuarios
        [HttpGet]
        [Route("listartodososusuarios")]
        public HttpResponseMessage ListarTodosUsuarios()
        {
            logger.Info("ListarTodosUsuarios - Iniciado");
            try
            {
                var retorno = _usuarioAppServico.BuscarTodos();

                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodosUsuarios - Sucesso!");

                    logger.Info("ListarTodosUsuarios - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosUsuarios - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";
                stats.Exception = e.Message;

                logger.Info("ListarTodosUsuarios - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosUsuarios - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ListarTodosUsuarios - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar usuário pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o usuário através do Id do mesmo</remarks>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/antiguera/admin/listarusuariosporid
        [HttpGet]
        [Route("listarusuariosporid")]
        public HttpResponseMessage ListarUsuariosPorId(int Id)
        {
            logger.Info("ListarUsuariosPorId - Iniciado");
            try
            {
                if(Id > 0)
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

                logger.Info("ListarUsuariosPorId - Finalizado");
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
        /// Listar usuário pelo Login ou Email
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Efetua a busca do usuário pelo Login ou Email</remarks>
        /// <param name="userData"></param>
        /// <returns></returns>
        // GET api/antiguera/admin/listarusuariosporloginouemail
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
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
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // POST api/antiguera/admin/inserirusuario
        [HttpPost]
        [Route("inserirusuario")]
        public HttpResponseMessage InserirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("InserirUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
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
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizarusuario
        [HttpPut]
        [Route("atualizarusuario")]
        public HttpResponseMessage AtualizarUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
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
        /// Atualizar administrador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário administrador passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizaradmin
        [HttpPut]
        [Route("atualizaradmin")]
        public HttpResponseMessage AtualizarAdmin([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarAdmin - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var usuario = Mapper.Map<UsuarioModel, Usuario>(usuarioModel);

                    _usuarioAppServico.Atualizar(usuario);

                    logger.Info("AtualizarAdmin - Sucesso!");

                    logger.Info("AtualizarAdmin - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarAdmin - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarAdmin - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarAdmin - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarAdmin - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar senha do usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza a senha do usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizarsenhausuario
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
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

        /// <summary>
        /// Atualizar senha do administrador
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza a senha do administrador passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizarsenhaadmin
        [HttpPut]
        [Route("atualizarsenhaadmin")]
        public HttpResponseMessage AtualizarSenhaAdmin([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarSenhaAdmin - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var senha = BCrypt.HashPassword(usuarioModel.Senha, BCrypt.GenerateSalt());

                    usuarioModel.Senha = senha;

                    _usuarioAppServico.AlterarSenha(usuarioModel.Id, usuarioModel.Senha);

                    logger.Info("AtualizarSenhaAdmin - Sucesso!");

                    logger.Info("AtualizarSenhaAdmin - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Senha alterada com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarSenhaAdmin - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarSenhaAdmin - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("AtualizarSenhaAdmin - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("AtualizarSenhaAdmin - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o usuário passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="usuarioModel"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/excluirusuario
        [HttpDelete]
        [Route("excluirusuario")]
        public HttpResponseMessage ExcluirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("ExcluirUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var usuario = Mapper.Map<UsuarioModel, Usuario>(usuarioModel);

                    _usuarioAppServico.Apagar(usuario);

                    logger.Info("ExcluirUsuario - Sucesso!");

                    logger.Info("ExcluirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário excluído com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ExcluirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ExcluirUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar usuários
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Deleta uma lista de usuarios passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids"></param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/apagarusuarios
        [HttpDelete]
        [Route("apagarusuarios")]
        public HttpResponseMessage ApagarUsuarios([FromBody] int[] Ids)
        {
            logger.Info("ApagarUsuarios - Iniciado");
            try
            {
                if (Ids.Count() > 0)
                {
                    _usuarioAppServico.ApagarUsuarios(Ids);

                    logger.Info("ApagarUsuarios - Sucesso!");

                    logger.Info("ApagarUsuarios - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Usuário(s) excluído(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarUsuarios - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Array preenchido incorretamente!";

                    logger.Info("ApagarUsuarios - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarUsuarios - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = "Ocorreu um erro ao processar sua solicitação!";
                stats.Exception = e.Message;

                logger.Info("ApagarUsuarios - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}