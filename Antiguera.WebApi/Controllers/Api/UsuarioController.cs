using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.Authorization;
using Antiguera.WebApi.Controllers.Api.Base;
using Antiguera.WebApi.Models;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers.Api
{
    [CustomAuthorize(Roles = "Usuário")]
    [RoutePrefix("api/antiguera/usuario")]
    public class UsuarioController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IAcessoAppServico _acessoAppServico;

        public UsuarioController(IUsuarioAppServico usuarioAppServico, IAcessoAppServico acessoAppServico)
        {
            _usuarioAppServico = usuarioAppServico;
            _acessoAppServico = acessoAppServico;
        }

        /// <summary>
        /// Listar usuário pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o usuário através do Id do mesmo</remarks>
        /// <param name="Id">Id do usuário</param>
        /// <returns></returns>
        // GET api/antiguera/usuario/listarusuariosporid?id={Id}
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
                    stats.Message = "Parâmetro incorreto!";

                    logger.Info("ListarUsuariosPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ListarUsuariosPorId - Finalizado");
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
        /// <param name="userData">Objeto do usuário</param>
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
                    stats.Message = "Parâmetro incorreto!";

                    logger.Info("ListarUsuariosPorLoginOuEmail - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Error("ListarUsuariosPorLoginOuEmail - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarUsuariosPorLoginOuEmail - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="usuarioModel">Objeto do usuário</param>
        /// <returns></returns>
        // POST api/antiguera/usuario/inserirusuario
        [HttpPost]
        [AllowAnonymous]
        [Route("inserirusuario")]
        public async Task<HttpResponseMessage> InserirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("InserirUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var acesso = _acessoAppServico.BuscarPorId(usuarioModel.AcessoId);
                    if (acesso != null)
                    {
                        var role = await RoleManager.FindByNameAsync(acesso.Nome);
                        if (role == null)
                        {
                            role = new IdentityRole(acesso.Nome);
                            await RoleManager.CreateAsync(role);
                        }

                        var user = await UserManager.FindByNameAsync(usuarioModel.Login);

                        if (user == null)
                        {
                            user = new ApplicationUser()
                            {
                                FirstName = usuarioModel.Nome.Split(' ')[0],
                                LastName = usuarioModel.Nome.Split(' ').LastOrDefault(),
                                Email = usuarioModel.Email,
                                UserName = usuarioModel.Login,
                                JoinDate = DateTime.Now
                            };

                            var result = await UserManager.CreateAsync(user, usuarioModel.Senha);

                            if (result.Succeeded)
                            {
                                usuarioModel.IdentityUserId = user.Id;
                            }
                            else
                            {
                                logger.Warn("InserirUsuario - Erro ao incluir usuário");
                                stats.Status = HttpStatusCode.BadRequest;
                                stats.Message = "Erro ao incluir usuário!";

                                logger.Info("InserirUsuario - Finalizado");
                                return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                            }

                            await UserManager.AddToRoleAsync(user.Id, role.Name);
                        }
                    }
                    else
                    {
                        logger.Error("InserirUsuario - Id de acesso não localizado");
                        stats.Status = HttpStatusCode.NotFound;
                        stats.Message = "Id de acesso não localizado!";

                        logger.Info("InserirUsuario - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.NotFound, stats);
                    }

                    usuarioModel.Created = DateTime.Now;

                    usuarioModel.Novo = true;

                    var usuario = Mapper.Map<UsuarioModel, Usuario>(usuarioModel);

                    _usuarioAppServico.Adicionar(usuario);

                    logger.Info("InserirUsuario - Sucesso!");

                    logger.Info("InserirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.Created, "Usuário incluído com sucesso!");
                }
                else
                {
                    logger.Warn("InserirUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("InserirUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel">Objeto do usuário</param>
        /// <returns></returns>
        // PUT api/antiguera/usuario/atualizarusuario
        [HttpPut]
        [Route("atualizarusuario")]
        public async Task<HttpResponseMessage> AtualizarUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var acesso = _acessoAppServico.BuscarPorId(usuarioModel.AcessoId);
                    if (acesso != null)
                    {
                        var role = await RoleManager.FindByNameAsync(acesso.Nome);
                        if (role == null)
                        {
                            role = new IdentityRole(acesso.Nome);
                            await RoleManager.CreateAsync(role);
                        }

                        var user = await UserManager.FindByNameAsync(usuarioModel.Login);

                        if (user != null)
                        {
                            user.FirstName = usuarioModel.Nome.Split(' ')[0];
                            user.LastName = usuarioModel.Nome.Split(' ').LastOrDefault();
                            user.Email = usuarioModel.Email;
                            user.UserName = usuarioModel.Login;

                            var roles = await UserManager.GetRolesAsync(user.Id);
                            await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                            await UserManager.AddToRoleAsync(user.Id, role.Name);

                            var result = await UserManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                usuarioModel.IdentityUserId = user.Id;
                            }
                            else
                            {
                                logger.Warn("AtualizarUsuario - Erro ao atualizar usuário!");
                                stats.Status = HttpStatusCode.BadRequest;
                                stats.Message = "Erro ao atualizar usuário!";

                                logger.Info("AtualizarUsuario - Finalizado");
                                return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                            }
                        }
                        else
                        {
                            throw new HttpResponseException(HttpStatusCode.NotFound);
                        }
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    usuarioModel.Modified = DateTime.Now;

                    usuarioModel.Novo = false;

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
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("AtualizarUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("AtualizarUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar senha do usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza a senha do usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel">Objeto do usuário</param>
        /// <returns></returns>
        // PUT api/antiguera/usuario/atualizarsenhausuario
        [HttpPut]
        [Route("atualizarsenhausuario")]
        public async Task<HttpResponseMessage> AtualizarSenhaUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarSenhaUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(usuarioModel.Login);

                    if (user != null)
                    {
                        await UserManager.RemovePasswordAsync(user.Id);

                        await UserManager.AddPasswordAsync(user.Id, usuarioModel.Senha);

                        logger.Info("AtualizarSenhaUsuario - Sucesso!");

                        logger.Info("AtualizarSenhaUsuario - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, "Senha alterada com sucesso!");
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("AtualizarSenhaUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Message = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarSenhaUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Error("AtualizarSenhaUsuario - Error: " + e);
                stats.Status = e.Response.StatusCode;
                stats.Message = "Nenhum registro encontrado!";

                logger.Info("AtualizarSenhaUsuario - Finalizado");
                return Request.CreateResponse(e.Response.StatusCode, stats);
            }

            catch (Exception e)
            {
                logger.Error("AtualizarSenhaUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Message = e.Message;

                logger.Info("AtualizarSenhaUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}
