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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers.Api
{
    [CustomAuthorize(Roles = "Administrador")]
    [RoutePrefix("api/antiguera/admin")]
    public class AdminController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static StatusCode stats = new StatusCode();
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IAcessoAppServico _acessoAppServico;

        public AdminController(IUsuarioAppServico usuarioAppServico, IAcessoAppServico acessoAppServico)
        {
            _usuarioAppServico = usuarioAppServico;
            _acessoAppServico = acessoAppServico;
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

                logger.Info("ListarTodosUsuarios - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosUsuarios - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

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
        /// <param name="Id">Id do usuário</param>
        /// <returns></returns>
        // GET api/antiguera/admin/listarusuariosporid?id={Id}
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
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

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
        /// <param name="userData">String do login ou email</param>
        /// <returns></returns>
        // GET api/antiguera/admin/listarusuariosporloginouemail?userData={userData}
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
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarUsuariosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("ListarUsuariosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar todos os acessos
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Listagem de todos os acessos dos usuários</remarks>
        /// <returns></returns>
        // GET api/antiguera/admin/listartodososacessos
        [HttpGet]
        [Route("listartodososacessos")]
        public HttpResponseMessage ListarTodosAcessos()
        {
            logger.Info("ListarTodosAcessos - Iniciado");
            try
            {
                var retorno = _acessoAppServico.BuscarTodos();

                if (retorno != null && retorno.Count() > 0)
                {
                    logger.Info("ListarTodosAcessos - Sucesso!");

                    logger.Info("ListarTodosAcessos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosAcessos - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("ListarTodosAcessos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarTodosAcessos - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("ListarTodosAcessos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Listar acesso pelo Id
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Retorna o acesso do usuário através do Id do mesmo</remarks>
        /// <param name="Id">Id do acesso</param>
        /// <returns></returns>
        // GET api/antiguera/admin/listaracessosporid?id={Id}
        [HttpGet]
        [Route("listaracessosporid")]
        public HttpResponseMessage ListarAcessosPorId(int Id)
        {
            logger.Info("ListarAcessosPorId - Iniciado");
            try
            {
                if (Id > 0)
                {
                    var acesso = _acessoAppServico.BuscarPorId(Id);

                    if (acesso != null)
                    {
                        logger.Info("ListarAcessosPorId - Sucesso!");

                        logger.Info("ListarAcessosPorId - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, acesso);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("ListarAcessosPorId - Parâmetro incorreto!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Parâmetro incorreto!";

                    logger.Info("ListarAcessosPorId - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ListarTodosAcessos - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("ListarTodosAcessos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ListarAcessosPorId - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("ListarAcessosPorId - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Pesquisar usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Efetua a pesquisa do usuário por qualquer dado inserido
        /// </remarks>
        /// <param name="busca">String de busca</param>
        /// <returns></returns>
        //GET api/antiguera/admin/pesquisausuario?busca={busca}
        [HttpGet]
        [Route("pesquisausuario")]
        public HttpResponseMessage PesquisaUsuario([FromUri] string busca)
        {
            logger.Info("PesquisaUsuario - Iniciar");
            try
            {
                if (!string.IsNullOrEmpty(busca))
                {
                    var isNumeric = int.TryParse(busca, out int n);

                    List<Usuario> retorno = null;

                    if (isNumeric)
                    {
                        retorno = _usuarioAppServico.BuscaQuery(x => x.Id == n
                                    || x.AcessoId == n).ToList();
                    }

                    else
                    {
                        retorno = _usuarioAppServico.BuscaQuery(x => x.Nome.Contains(busca) ||
                                    x.Nome == busca || RemoveDiacritics(x.Nome).ToLower().Contains(busca.ToLower()) ||
                                    x.Email.Contains(busca) || RemoveDiacritics(x.Acesso.Nome).ToLower().Contains(busca.ToLower())
                                    || RemoveDiacritics(x.Login).ToLower().Contains(busca.ToLower())).ToList();
                    }

                    if (retorno != null && retorno.Count > 0)
                    {
                        logger.Info("PesquisaUsuario - Sucesso!");

                        logger.Info("PesquisaUsuario - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, retorno);
                    }

                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("PesquisaUsuario - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("PesquisaUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Warn("PesquisaUsuario - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("PesquisaUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }
            catch (Exception e)
            {
                logger.Error("PesquisaUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("PesquisaUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Pesquisar acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Efetua a pesquisa do acesso por qualquer dado inserido
        /// </remarks>
        /// <param name="busca">String de busca</param>
        /// <returns></returns>
        //GET api/antiguera/admin/pesquisaacesso?busca={busca}
        [HttpGet]
        [Route("pesquisaacesso")]
        public HttpResponseMessage PesquisaAcesso([FromUri] string busca)
        {
            logger.Info("PesquisaAcesso - Iniciar");
            try
            {
                if (!string.IsNullOrEmpty(busca))
                {
                    var isNumeric = int.TryParse(busca, out int n);

                    List<Acesso> retorno = null;

                    if (isNumeric)
                    {
                        retorno = _acessoAppServico.BuscaQuery(x => x.Id == n).ToList();
                    }

                    else
                    {
                        retorno = _acessoAppServico.BuscaQuery(x => x.Nome.ToLower() == busca.ToLower()
                        || RemoveDiacritics(x.Nome).ToLower().Contains(busca.ToLower())).ToList();
                    }

                    if (retorno != null && retorno.Count > 0)
                    {
                        logger.Info("PesquisaAcesso - Sucesso!");

                        logger.Info("PesquisaAcesso - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.OK, retorno);
                    }

                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    logger.Warn("PesquisaAcesso - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("PesquisaAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }
            catch (HttpResponseException e)
            {
                logger.Warn("PesquisaAcesso - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("PesquisaAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }
            catch (Exception e)
            {
                logger.Error("PesquisaAcesso - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("PesquisaAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Login no Admin
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Login Admin através da base identity passando no body o objeto do usuário</remarks>
        /// <param name="model">Objeto do login</param>
        /// <returns></returns>
        // POST api/antiguera/admin/login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<HttpResponseMessage> LoginAdmin([FromBody] LoginModel model)
        {
            logger.Info("LoginAdmin - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {                    
                    var user = await UserManager.FindAsync(model.UserName, model.Password);

                    if (user != null)
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
                stats.Mensagem = e.Message;

                logger.Info("LoginAdmin - Finalizado");
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
        // POST api/antiguera/admin/inserirusuario
        [HttpPost]
        [Route("inserirusuario")]
        public async Task<HttpResponseMessage> InserirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("InserirUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var acesso = _acessoAppServico.BuscarPorId(usuarioModel.AcessoId);
                    if(acesso != null)
                    {
                        var role = await RoleManager.FindByNameAsync(acesso.Nome);
                        if(role == null)
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

                            await UserManager.CreateAsync(user, usuarioModel.Senha);

                            await UserManager.AddToRoleAsync(user.Id, role.Name);
                        }
                    }
                    else
                    {
                        logger.Error("InserirUsuario - Id de acesso não localizado");
                        stats.Status = HttpStatusCode.NotFound;
                        stats.Mensagem = "Id de acesso não localizado!";

                        logger.Info("InserirUsuario - Finalizado");
                        return Request.CreateResponse(HttpStatusCode.NotFound, stats);
                    }

                    var senha = BCrypt.HashPassword(usuarioModel.Senha, BCrypt.GenerateSalt());

                    usuarioModel.Senha = senha;

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
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("InserirUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Inserir acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Insere um novo acesso de usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="acessoModel">Objeto do acesso</param>
        /// <returns></returns>
        // POST api/antiguera/admin/inseriracesso
        [HttpPost]
        [Route("inseriracesso")]
        public async Task<HttpResponseMessage> InserirAcesso([FromBody] AcessoModel acessoModel)
        {
            logger.Info("InserirAcesso - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await RoleManager.FindByNameAsync(acessoModel.Nome);
                    if (role == null)
                    {
                        role = new IdentityRole(acessoModel.Nome);
                        await RoleManager.CreateAsync(role);
                    }

                    acessoModel.Created = DateTime.Now;

                    acessoModel.Novo = true;

                    var acesso = Mapper.Map<AcessoModel, Acesso>(acessoModel);

                    _acessoAppServico.Adicionar(acesso);

                    logger.Info("InserirAcesso - Sucesso!");

                    logger.Info("InserirAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.Created, "Acesso incluído com sucesso!");
                }
                else
                {
                    logger.Warn("InserirAcesso - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("InserirAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("InserirAcesso - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("InserirAcesso - Finalizado");
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
        // PUT api/antiguera/admin/atualizarusuario
        [HttpPut]
        [Route("atualizarusuario")]
        public async Task<HttpResponseMessage> AtualizarUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
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

                        if(user != null)
                        {
                            user.FirstName = usuarioModel.Nome.Split(' ')[0];
                            user.LastName = usuarioModel.Nome.Split(' ').LastOrDefault();
                            user.Email = usuarioModel.Email;
                            user.UserName = usuarioModel.Login;

                            var roles = await UserManager.GetRolesAsync(user.Id);
                            await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                            await UserManager.AddToRoleAsync(user.Id, role.Name);

                            await UserManager.UpdateAsync(user);
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
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarUsuario - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("AtualizarUsuario - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("AtualizarUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("AtualizarUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("AtualizarUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar senha
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza a senha do usuário passando o objeto no body da requisição pelo método PUT</remarks>
        /// <param name="usuarioModel">Objeto do usuário</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizarsenha
        [HttpPut]
        [Route("atualizarsenha")]
        public async Task<HttpResponseMessage> AtualizarSenhaUsuario([FromBody]UsuarioModel usuarioModel)
        {
            logger.Info("AtualizarSenhaUsuario - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(usuarioModel.Login);

                    if(user != null)
                    {
                        await UserManager.RemovePasswordAsync(user.Id);

                        await UserManager.AddPasswordAsync(user.Id, usuarioModel.Senha);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    usuarioModel.Modified = DateTime.Now;

                    usuarioModel.Novo = false;

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

            catch (HttpResponseException e)
            {
                logger.Error("AtualizarSenhaUsuario - Error: " + e);
                stats.Status = e.Response.StatusCode;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("AtualizarSenhaUsuario - Finalizado");
                return Request.CreateResponse(e.Response.StatusCode, stats);
            }

            catch (Exception e)
            {
                logger.Error("AtualizarSenhaUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("AtualizarSenhaUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Atualizar acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Atualiza o acesso de usuário passando um objeto no body da requisição no método POST</remarks>
        /// <param name="acessoModel">Objeto do acesso</param>
        /// <returns></returns>
        // PUT api/antiguera/admin/atualizaracesso
        [HttpPut]
        [Route("atualizaracesso")]
        public async Task<HttpResponseMessage> AtualizarAcesso([FromBody] AcessoModel acessoModel)
        {
            logger.Info("AtualizarAcesso - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = _acessoAppServico.BuscarPorId(acessoModel.Id);
                    if (obj != null)
                    {
                        var role = await RoleManager.FindByNameAsync(obj.Nome);
                        if (role != null)
                        {
                            role.Name = acessoModel.Nome;
                            await RoleManager.UpdateAsync(role);
                        }
                        else
                        {
                            role = new IdentityRole(acessoModel.Nome);
                            await RoleManager.CreateAsync(role);
                        }
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    acessoModel.Modified = DateTime.Now;

                    acessoModel.Novo = false;

                    var acesso = Mapper.Map<AcessoModel, Acesso>(acessoModel);

                    _acessoAppServico.Atualizar(acesso);

                    logger.Info("AtualizarAcesso - Sucesso!");

                    logger.Info("AtualizarAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Acesso atualizado com sucesso!");
                }
                else
                {
                    logger.Warn("AtualizarAcesso - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("AtualizarAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("AtualizarAcesso - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("AtualizarAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("InserirAcesso - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("InserirAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir usuário
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o usuário passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="usuarioModel">Objeto do usuário</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/excluirusuario
        [HttpDelete]
        [Route("excluirusuario")]
        public async Task<HttpResponseMessage> ExcluirUsuario([FromBody] UsuarioModel usuarioModel)
        {
            logger.Info("ExcluirUsuario - Iniciado");
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(usuarioModel.Login);

                    if (user != null)
                    {
                        await UserManager.DeleteAsync(user);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

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

            catch (HttpResponseException e)
            {
                logger.Warn("ExcluirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("ExcluirUsuario - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ExcluirUsuario - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

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
        /// <param name="Ids">Ids de usuários</param>
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
                stats.Mensagem = e.Message;

                logger.Info("ApagarUsuarios - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Excluir acesso
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Exclui o acesso usuário passando o objeto no body da requisição pelo método DELETE</remarks>
        /// <param name="acessoModel">Objeto do acesso</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/excluiracesso
        [HttpDelete]
        [Route("excluiracesso")]
        public async Task<HttpResponseMessage> ExcluirAcesso([FromBody] AcessoModel acessoModel)
        {
            logger.Info("ExcluirAcesso - Iniciado");
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await RoleManager.FindByNameAsync(acessoModel.Nome);
                    if (role != null)
                    {
                        await RoleManager.DeleteAsync(role);
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    var acesso = Mapper.Map<AcessoModel, Acesso>(acessoModel);

                    _acessoAppServico.Apagar(acesso);

                    logger.Info("ExcluirAcesso - Sucesso!");

                    logger.Info("ExcluirAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Acesso excluído com sucesso!");
                }
                else
                {
                    logger.Warn("ExcluirAcesso - Por favor, preencha os campos corretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Por favor, preencha os campos corretamente!";

                    logger.Info("ExcluirAcesso - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (HttpResponseException e)
            {
                logger.Warn("ExcluirAcesso - Error: " + e);
                stats.Status = HttpStatusCode.NotFound;
                stats.Mensagem = "Nenhum registro encontrado!";

                logger.Info("ExcluirAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.NotFound, stats);
            }

            catch (Exception e)
            {
                logger.Error("ExcluirAcesso - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("ExcluirAcesso - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }

        /// <summary>
        /// Apagar acessos
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Deleta uma lista de acessos de usuarios passando um array de Ids no body da requisição</remarks>
        /// <param name="Ids">Ids de acessos</param>
        /// <returns></returns>
        // DELETE api/antiguera/admin/apagaracessos
        [HttpDelete]
        [Route("apagaracessos")]
        public HttpResponseMessage ApagarAcessos([FromBody] int[] Ids)
        {
            logger.Info("ApagarAcessos - Iniciado");
            try
            {
                if (Ids.Count() > 0)
                {
                    _acessoAppServico.ApagarAcessos(Ids);

                    logger.Info("ApagarAcessos - Sucesso!");

                    logger.Info("ApagarAcessos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.OK, "Acesso(s) excluído(s) com sucesso!");
                }
                else
                {
                    logger.Warn("ApagarAcessos - Array preenchido incorretamente!");
                    stats.Status = HttpStatusCode.BadRequest;
                    stats.Mensagem = "Array preenchido incorretamente!";

                    logger.Info("ApagarAcessos - Finalizado");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, stats);
                }
            }

            catch (Exception e)
            {
                logger.Error("ApagarAcessos - Error: " + e);
                stats.Status = HttpStatusCode.InternalServerError;
                stats.Mensagem = e.Message;

                logger.Info("ApagarAcessos - Finalizado");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, stats);
            }
        }
    }
}