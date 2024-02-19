using Antiguera.Api.Models;
using Antiguera.Api.Utils;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Servicos.IdentityConfiguration;
using Antiguera.Servicos.Servicos.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Antiguera.Api.Admin.Controllers
{
    [RoutePrefix("Api/Account")]
    public class AccountController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly AccountServico _accountServico;

        public AccountController(AccountServico accountServico)
        {
            _accountServico = accountServico;
        }

        #region LOGIN
        /// <summary>
        /// Logout
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>Deslogar do Sistema</remarks>
        /// <returns></returns>
        // POST: /Account/Logout
        [Route("Logout")]
        public HttpResponseMessage Logout()
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                ApplicationOAuthProvider.Logout(Request.GetOwinContext(), CookieAuthenticationDefaults.AuthenticationType);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");

                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region LOGINS EXTERNOS
        /// <summary>
        /// Login Externo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <remarks>Efetuar login com provedores externos</remarks>
        /// <returns></returns>
        // GET: /Account/LoginExterno
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [HttpGet]
        [Route("LoginExterno", Name = "LoginExterno")]
        public async Task<HttpResponseMessage> LoginExterno(string provider, string error = null)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                if (error != null)
                {
                    throw new Exception(error);
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                await ApplicationOAuthProvider.ExternalLogin(Request.GetOwinContext(), User, provider);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.Fatal(ex, "Erro Fatal");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
            catch (ApplicationException ex)
            {
                _logger.Fatal(ex, "Erro Fatal");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Obter Logins Externos
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <remarks>Obtém todos logins externos vinculados</remarks>
        /// <returns></returns>
        // GET: /Account/ObterLoginsExternos
        [HttpGet]
        [Route("ObterLoginsExternos")]
        public HttpResponseMessage ObterLoginsExternos(string returnUrl, bool generateState = false)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                IEnumerable<AuthenticationDescription> descriptions = ApplicationOAuthProvider.ObterAuthenticationTypes(Request.GetOwinContext());
                List<ExternalLoginModel> logins = new List<ExternalLoginModel>();

                string state;

                if (generateState)
                {
                    const int strengthInBits = 256;
                    state = RandomOAuthStateGenerator.Generate(strengthInBits);
                }
                else
                {
                    state = null;
                }

                foreach (AuthenticationDescription description in descriptions)
                {
                    ExternalLoginModel login = new ExternalLoginModel
                    {
                        Name = description.Caption,
                        Url = Url.Route("LoginExterno", new
                        {
                            provider = description.AuthenticationType,
                            response_type = "token",
                            client_id = AppBuilderConfiguration.PublicClientId,
                            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                            state = state
                        }),
                        State = state
                    };
                    logins.Add(login);
                }


                return Request.CreateResponse(HttpStatusCode.OK, logins);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Adicionar Login Externo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="model"></param>
        /// <remarks>Adiciona login externo</remarks>
        /// <returns></returns>
        // POST: /Account/AdicionarLoginExterno
        [HttpPost]
        [Route("AdicionarLoginExterno")]
        public async Task<HttpResponseMessage> AdicionarLoginExterno(AddExternalLoginBindingModel model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                ApplicationOAuthProvider.Logout(Request.GetOwinContext(), DefaultAuthenticationTypes.ExternalCookie);

                var result = await _accountServico.AdicionarLoginExternoAsync(User.Identity.GetUserId(), model.ExternalAccessToken);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Adicionar Usuário ao Login Externo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="model"></param>
        /// <remarks>Adiciona usuário ao provedor de login externo</remarks>
        /// <returns></returns>
        // POST: /Account/AdicionarUsuarioLoginExterno
        [HttpPost]
        [Route("AdicionarUsuarioLoginExterno")]
        public async Task<HttpResponseMessage> AdicionarUsuarioLoginExterno(RegisterExternalBindingModel model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                var result = await ApplicationOAuthProvider.RegisterExternal(Request.GetOwinContext(), model.Email, model.Email);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Remover Login Externo
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="model"></param>
        /// <remarks>Remove provedor de login externo</remarks>
        /// <returns></returns>
        // POST: /Account/RemoverLoginExterno
        [HttpPost]
        [Route("RemoverLoginExterno")]
        public async Task<HttpResponseMessage> RemoverLoginExterno(RemoveLoginBindingModel model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                var result = await _accountServico.RemoverLoginExternoAsync(User.Identity.GetUserId(), model.LoginProvider, model.ProviderKey);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region SENHA
        /// <summary>
        /// Alterar Senha
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="changePasswordBinding"></param>
        /// <remarks>Alterar senha do usuário</remarks>
        /// <returns></returns>
        // POST /Account/AlterarSenha
        [Route("AlterarSenha")]
        public async Task<HttpResponseMessage> AlterarSenha(ChangePasswordBindingDTO changePasswordBinding)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var result = await _accountServico.AlterarSenhaAsync(changePasswordBinding);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Esqueci Minha Senha
        /// </summary>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="model"></param>
        /// <remarks>Enviar pedido de esquecimento de senha</remarks>
        /// <returns></returns>
        // POST: /Account/EsqueciMinhaSenha
        [HttpPost]
        [AllowAnonymous]
        [Route("EsqueciMinhaSenha")]
        public async Task<HttpResponseMessage> EsqueciMinhaSenha(ForgotPasswordModel model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                // Exija que o usuário efetue login via nome de usuário/senha ou login externo
                var dto = await _accountServico.GerarTokenRecuperacaoSenhaAsync(model.Email);
                if (dto != null)
                {
                    dto.CallBackUrl = string.Format(model.CallBackUrl, dto.UserId, dto.Code);

                    await _accountServico.EnviarEmailRecuperacaoSenhaAsync(dto);

                    _logger.Info(action + " - Sucesso!");

                    _logger.Info(action + " - Finalizado");
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Email de recuperação de senha enviado com sucesso!");
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
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Recuperar Senha
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="model"></param>
        /// <remarks>Recuperação de senha do usuário</remarks>
        /// <returns></returns>
        // POST: /Account/RecuperarSenha
        [HttpPost]
        [Route("RecuperarSenha")]
        public async Task<HttpResponseMessage> RecuperarSenha(ResetPasswordDTO model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                var result = await _accountServico.RecuperarSenhaAsync(model);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }


                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);

            }

            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion
    }
}
