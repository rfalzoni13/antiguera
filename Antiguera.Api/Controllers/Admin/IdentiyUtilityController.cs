using Antiguera.Api.Models;
using Antiguera.Api.Utils;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Servicos.Servicos.Identity;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Antiguera.Api.Controllers.Admin
{
    [RoutePrefix("Api/IdentityUtility")]
    public class IdentiyUtilityController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IdentityUtilityServico _identityUtilityServico;

        public IdentiyUtilityController(IdentityUtilityServico identityUtilityServico)
        {
            _identityUtilityServico = identityUtilityServico;
        }

        #region DOIS FATORES
        /// <summary>
        /// Obter Autenticação Dois Fatores
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="email"></param>
        /// <param name="returnUrl"></param>
        /// <remarks>Obtém as opções de autencicação de dois fatores</remarks>
        /// <returns></returns>
        // GET: /IdentityUtitlity/ObterAutenticacaoDoisFatores
        [HttpGet]
        [Route("ObterAutenticacaoDoisFatores")]
        public async Task<HttpResponseMessage> ObterAutenticacaoDoisFatores(string email, string returnUrl = null)
        {
            string action = this.ActionContext.ActionDescriptor.ActionName;

            try
            {
                var userFactors = await _identityUtilityServico.ObterAutenticacaoDoisFatoresAsync(email);

                return Request.CreateResponse(HttpStatusCode.OK, userFactors);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return ResponseMessageHelper.RetornoExceptionErroInterno(ex, Request, _logger, action);
            }
        }

        /// <summary>
        /// Enviar código de dois fatores
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="sendCodeModel"></param>
        /// <remarks>Efetua o envio do código de dois fatores</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/EnviarCodigoDoisFatores
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

                await _identityUtilityServico.EnviarCodigoAsync(sendCodeDTO);

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

        /// <summary>
        /// Verificar código de dois fatores
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="verifiyCodeModel"></param>
        /// <remarks>Efetua verificação de código de dois fatores</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/VerificarCodigoDoisFatores
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

                var retornoCodigo = await _identityUtilityServico.VerificarCodigoAsync(verifiyCodeDTO);

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

        #region CONFIRMAÇÃO DE EMAIL E TELEFONE
        /// <summary>
        /// Enviar Código de Confirmação de E-mail
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="generateTokenEmailModel"></param>
        /// <remarks>Envio de código de confirmação de e-mail</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/EnviarCodigoConfirmacaoEmail
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

                await _identityUtilityServico.EnviarCodigoConfirmacaoEmailAsync(generateTokenEmailDTO);

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

        /// <summary>
        /// Enviar Código de Confirmação de Telefone
        /// </summary>
        /// <response code="500">Internal Server Error</response>
        /// <param name="generateTokenPhoneModel"></param>
        /// <remarks>Envio de código de confirmação de telefone</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/EnviarCodigoConfirmacaoTelefone
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

                await _identityUtilityServico.EnviarCodigoConfirmacaoTelefoneAsync(generateTokenPhoneDTO);

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

        /// <summary>
        /// Verificar Código de Confirmação de E-mail
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="confirmEmailCodeModel"></param>
        /// <remarks>Verificação de código de confirmação do e-mail do usuário</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/VerificarCodigoConfirmacaoEmail
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

                var result = await _identityUtilityServico.VerificarCodigoConfirmacaoEmailAsync(confirmEmailCodeDTO);
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

        /// <summary>
        /// Verificar Código de Confirmação de Telefone
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="confirmPhoneCodeModel"></param>
        /// <remarks>Verificação de código de confirmação do telefone do usuário</remarks>
        /// <returns></returns>
        // POST: /IdentityUtitlity/VerificarCodigoConfirmacaoTelefone
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

                var result = await _identityUtilityServico.VerificarCodigoConfirmacaoTelefoneAsync(confirmPhoneCodeDTO);
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

    }
}
