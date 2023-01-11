using Antiguera.Api.Models;
using Antiguera.Api.Utils;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Autorizacao;
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
        private readonly IAccountServico _accountServico;

        public AccountController(IAccountServico accountServico)
        {
            _accountServico = accountServico;
        }

        #region LOGIN
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

                var result = await _accountServico.AdicionarLoginExterno(User.Identity.GetUserId(), model.ExternalAccessToken);
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

        // POST: /Account/RemoverLoginExterno
        [HttpPost]
        [Route("RemoverLoginExterno")]
        public async Task<HttpResponseMessage> RemoverLoginExterno(RemoveLoginBindingModel model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                _logger.Info(action + " - Iniciado");

                var result = await _accountServico.RemoverLoginExterno(User.Identity.GetUserId(), model.LoginProvider, model.ProviderKey);
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

        #region DOIS FATORES
        // GET: /Account/ObterAutenticacaoDoisFatores
        [HttpGet]
        [Route("ObterAutenticacaoDoisFatores")]
        public async Task<HttpResponseMessage> ObterAutenticacaoDoisFatores(string email, string returnUrl = null)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                var userFactors = await _accountServico.ObterAutenticacaoDoisFatores(email);

                return Request.CreateResponse(HttpStatusCode.OK, userFactors);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // POST: /Account/EnviarCodigoDoisFatores
        [CustomAuthorize]
        [HttpPost]
        [Route("EnviarCodigoDoisFatores")]
        public async Task<HttpResponseMessage> EnviarCodigoDoisFatores(SendCodeModel sendCodeModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var sendCodeDTO = new SendCodeDTO
                {
                    UserId = sendCodeModel.UserId,
                    SelectedProvider = sendCodeModel.SelectedProvider
                };

                await _accountServico.EnviarCodigo(sendCodeDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Código enviado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // POST: /Account/VerificarCodigoDoisFatores
        [CustomAuthorize]
        [HttpPost]
        [Route("VerificarCodigoDoisFatores")]
        public async Task<HttpResponseMessage> VerificarCodigoDoisFatores(VerifyCodeModel verifiyCodeModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new Exception("Usuário não autenticado!");
                }
                _logger.Info(action + " - Iniciado");
                var verifiyCodeDTO = new VerifyCodeDTO
                {
                    UserId = verifiyCodeModel.UserId,
                    Code = verifiyCodeModel.Code,
                    Provider = verifiyCodeModel.Provider,
                    ReturnUrl = verifiyCodeModel.ReturnUrl
                };

                var retornoCodigo = await _accountServico.VerificarCodigo(verifiyCodeDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, retornoCodigo);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region CADASTRO
        // POST: /Account/Cadastrar
        [HttpPost]
        [Route("Cadastrar")]
        public async Task<HttpResponseMessage> Cadastrar(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var applicationUserRegisterDTO = new ApplicationUserRegisterDTO
                {
                    Nome = applicationUserRegisterModel.Nome,
                    Email = applicationUserRegisterModel.Email,
                    Login = applicationUserRegisterModel.Login,
                    PathFoto = applicationUserRegisterModel.PathFoto,
                    Genero = applicationUserRegisterModel.Genero,
                    DataNascimento = applicationUserRegisterModel.DataNascimento.Date,
                    IdAcesso = applicationUserRegisterModel.IdAcesso,
                    Senha = applicationUserRegisterModel.Senha,
                    AcceptTerms = applicationUserRegisterModel.AcceptTerms
                };

                await _accountServico.Adicionar(applicationUserRegisterDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // PUT: /Account/Atualizar
        [HttpPut]
        [Authorize]
        [Route("Atualizar")]
        public async Task<HttpResponseMessage> Atualizar(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var applicationUserRegisterDTO = new ApplicationUserRegisterDTO
                {
                    ID = applicationUserRegisterModel.ID,
                    Nome = applicationUserRegisterModel.Nome,
                    Email = applicationUserRegisterModel.Email,
                    Login = applicationUserRegisterModel.Login,
                    DataNascimento = applicationUserRegisterModel.DataNascimento.Date,
                    PathFoto = applicationUserRegisterModel.PathFoto,
                    IdAcesso = applicationUserRegisterModel.IdAcesso,
                    Senha = applicationUserRegisterModel.Senha,
                    AcceptTerms = applicationUserRegisterModel.AcceptTerms
                };

                await _accountServico.Atualizar(applicationUserRegisterDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        //DELETE: /Account/Apagar
        [HttpDelete]
        [CustomAuthorize]
        [Route("Apagar")]
        public async Task<HttpResponseMessage> Apagar(ApplicationUserRegisterModel applicationUserRegisterModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var applicationUserRegisterDTO = new ApplicationUserRegisterDTO
                {
                    ID = applicationUserRegisterModel.ID,
                    Nome = applicationUserRegisterModel.Nome,
                    Email = applicationUserRegisterModel.Email,
                    Login = applicationUserRegisterModel.Login,
                    PathFoto = applicationUserRegisterModel.PathFoto,
                    DataNascimento = applicationUserRegisterModel.DataNascimento.Date,
                    IdAcesso = applicationUserRegisterModel.IdAcesso,
                    Senha = applicationUserRegisterModel.Senha,
                    AcceptTerms = applicationUserRegisterModel.AcceptTerms
                };

                await _accountServico.Apagar(applicationUserRegisterDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Usuário deletado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region CONFIRMAÇÃO DE EMAIL E TELEFONE
        // POST: /Account/EnviarCodigoConfirmacaoEmail
        [HttpPost]
        [Route("EnviarCodigoConfirmacaoEmail")]
        public async Task<HttpResponseMessage> EnviarCodigoConfirmacaoEmail(GenerateTokenEmailModel generateTokenEmailModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var generateTokenEmailDTO = new GenerateTokenEmailDTO
                {
                    UserId = generateTokenEmailModel.UserId,
                    Url = generateTokenEmailModel.Url
                };

                await _accountServico.EnviarCodigoConfirmacaoEmail(generateTokenEmailDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Código enviado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // POST: /Account/EnviarCodigoConfirmacaoTelefone
        [HttpPost]
        [Route("EnviarCodigoConfirmacaoTelefone")]
        public async Task<HttpResponseMessage> EnviarCodigoConfirmacaoTelefone(GenerateTokenPhoneModel generateTokenPhoneModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var generateTokenPhoneDTO = new GenerateTokenPhoneDTO
                {
                    UserId = generateTokenPhoneModel.UserId,
                    Phone = generateTokenPhoneModel.Phone
                };

                await _accountServico.EnviarCodigoConfirmacaoTelefone(generateTokenPhoneDTO);

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Código enviado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // POST: /Account/VerificarCodigoConfirmacaoEmail
        [HttpPost]
        [Route("VerificarCodigoConfirmacaoEmail")]
        public async Task<HttpResponseMessage> VerificarCodigoConfirmacaoEmail(ConfirmEmailCodeModel confirmEmailCodeModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var confirmEmailCodeDTO = new ConfirmEmailCodeDTO
                {
                    UserId = confirmEmailCodeModel.UserId,
                    Code = confirmEmailCodeModel.Code
                };

                var result = await _accountServico.VerificarCodigoConfirmacaoEmail(confirmEmailCodeDTO);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Email confirmado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        // POST: /Account/VerificarCodigoConfirmacaoTelefone
        [HttpPost]
        [Route("VerificarCodigoConfirmacaoTelefone")]
        public async Task<HttpResponseMessage> VerificarCodigoConfirmacaoTelefone(ConfirmPhoneCodeModel confirmPhoneCodeModel)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var confirmPhoneCodeDTO = new ConfirmPhoneCodeDTO
                {
                    UserId = confirmPhoneCodeModel.UserId,
                    Phone = confirmPhoneCodeModel.Phone,
                    Code = confirmPhoneCodeModel.Code
                };

                var result = await _accountServico.VerificarCodigoConfirmacaoTelefone(confirmPhoneCodeDTO);
                if (!result.Succeeded)
                {
                    return ResponseMessageHelper.RetornoErrorResult(Request, _logger, action, result.Errors);
                }

                _logger.Info(action + " - Sucesso!");

                _logger.Info(action + " - Finalizado");
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Telefone confirmado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }
        #endregion

        #region SENHA
        // POST /Account/AlterarSenha
        [Route("AlterarSenha")]
        public async Task<HttpResponseMessage> AlterarSenha(ChangePasswordBindingDTO changePasswordBinding)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;
            try
            {
                _logger.Info(action + " - Iniciado");
                var result = await _accountServico.AlterarSenha(changePasswordBinding);
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
                var dto = await _accountServico.GerarTokenRecuperacaoSenha(model.Email);
                if (dto != null)
                {
                    dto.CallBackUrl = string.Format(model.CallBackUrl, dto.UserId, dto.Code);

                    await _accountServico.EnviarEmailRecuperacaoSenha(dto);

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

        // POST: /Account/RecuperarSenha
        [HttpPost]
        [Route("RecuperarSenha")]
        public async Task<HttpResponseMessage> RecuperarSenha(ResetPasswordDTO model)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                var result = await _accountServico.RecuperarSenha(model);
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
