using Antiguera.Dominio.DTO.Identity;
using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.IdentityConfiguration;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.Servicos.Servicos.Identity
{
    public class IdentityUtilityServico : IDisposable
    {
        #region Atributos
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
        #endregion

        #region Services
        public void EnviarCodigo(SendCodeDTO sendCode)
        {
            try
            {
                // Gerar o token e enviá-lo
                ApplicationUser user = UserManager.FindById(sendCode.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = UserManager.GenerateTwoFactorToken(sendCode.UserId, sendCode.SelectedProvider);

                var result = UserManager.NotifyTwoFactorToken(sendCode.UserId, sendCode.SelectedProvider, token);
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Erro ao enviar código!");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EnviarCodigoConfirmacaoEmail(GenerateTokenEmailDTO generateTokenEmailDTO)
        {
            try
            {
                // Gerar o token de confirmação e enviá-lo
                ApplicationUser user = UserManager.FindById(generateTokenEmailDTO.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = UserManager.GenerateEmailConfirmationToken(generateTokenEmailDTO.UserId);

                Uri uri;

                if (Uri.TryCreate(generateTokenEmailDTO.Url, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttp)
                {
                    var uriBuilder = new UriBuilder(uri);

                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["userId"] = generateTokenEmailDTO.UserId;
                    query["code"] = StringHelper.Base64ForUrlEncode(token);

                    uriBuilder.Query = query.ToString();

                    UserManager.SendEmail(generateTokenEmailDTO.UserId, "Confirmação de Email!", string.Format("Olá, para confirmar seu e-mail, clique neste <a href='{0}' />link</a>!", uriBuilder.ToString()));
                }
                else
                {
                    throw new ApplicationException("Url inválida!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EnviarCodigoConfirmacaoTelefone(GenerateTokenPhoneDTO generateTokenPhoneDTO)
        {
            try
            {
                // Gerar o token de confirmação e enviá-lo
                ApplicationUser user = UserManager.FindById(generateTokenPhoneDTO.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = UserManager.GenerateChangePhoneNumberToken(generateTokenPhoneDTO.UserId, generateTokenPhoneDTO.Phone);

                UserManager.SendEmail(generateTokenPhoneDTO.UserId, "Confirmação de Telefone!", string.Format("Olá, seu código para confirmar seu telefone é: {0}", token));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ObterAutenticacaoDoisFatores(string email)
        {
            var user = UserManager.FindByEmail(email);

            if (user == null)
            {
                throw new ApplicationException("Usuário não encontrado!");
            }

            var userFactors = UserManager.GetValidTwoFactorProviders(user.Id);

            return userFactors.ToList();
        }

        public ReturnVerifyCodeDTO VerificarCodigo(VerifyCodeDTO verifiyCode)
        {
            try
            {
                // Verificar token recebido!
                ApplicationUser user = UserManager.FindById(verifiyCode.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                bool token = UserManager.VerifyTwoFactorToken(verifiyCode.UserId, verifiyCode.Provider, verifiyCode.Code);

                if (!token)
                {
                    throw new ApplicationException("Código inválido!");
                }

                return new ReturnVerifyCodeDTO
                {
                    ReturnUrl = verifiyCode.ReturnUrl
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityResultCodeDTO VerificarCodigoConfirmacaoEmail(ConfirmEmailCodeDTO confirmEmailCodeDTO)
        {
            try
            {
                var identityResult = UserManager.ConfirmEmail(confirmEmailCodeDTO.UserId, StringHelper.Base64ForUrlDecode(confirmEmailCodeDTO.Code));
                var result = new IdentityResultCodeDTO
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityResultCodeDTO VerificarCodigoConfirmacaoTelefone(ConfirmPhoneCodeDTO confirmPhoneCodeDTO)
        {
            try
            {
                var identityResult = UserManager.ChangePhoneNumber(confirmPhoneCodeDTO.UserId, confirmPhoneCodeDTO.Phone, confirmPhoneCodeDTO.Code);
                var result = new IdentityResultCodeDTO
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Async Services
        public async Task EnviarCodigoAsync(SendCodeDTO sendCode)
        {
            try
            {
                // Gerar o token e enviá-lo
                ApplicationUser user = await UserManager.FindByIdAsync(sendCode.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = await UserManager.GenerateTwoFactorTokenAsync(sendCode.UserId, sendCode.SelectedProvider);

                var result = await UserManager.NotifyTwoFactorTokenAsync(sendCode.UserId, sendCode.SelectedProvider, token);
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Erro ao enviar código!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnviarCodigoConfirmacaoEmailAsync(GenerateTokenEmailDTO generateTokenEmailDTO)
        {
            try
            {
                // Gerar o token de confirmação e enviá-lo
                ApplicationUser user = await UserManager.FindByIdAsync(generateTokenEmailDTO.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = await UserManager.GenerateEmailConfirmationTokenAsync(generateTokenEmailDTO.UserId);

                Uri uri;

                if (Uri.TryCreate(generateTokenEmailDTO.Url, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttp)
                {
                    var uriBuilder = new UriBuilder(uri);

                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["userId"] = generateTokenEmailDTO.UserId;
                    query["code"] = StringHelper.Base64ForUrlEncode(token);

                    uriBuilder.Query = query.ToString();

                    await UserManager.SendEmailAsync(generateTokenEmailDTO.UserId, "Confirmação de Email!", string.Format("Olá, para confirmar seu e-mail, clique neste <a href='{0}' />link</a>!", uriBuilder.ToString()));
                }
                else
                {
                    throw new ApplicationException("Url inválida!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnviarCodigoConfirmacaoTelefoneAsync(GenerateTokenPhoneDTO generateTokenPhoneDTO)
        {
            try
            {
                // Gerar o token de confirmação e enviá-lo
                ApplicationUser user = await UserManager.FindByIdAsync(generateTokenPhoneDTO.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                var token = await UserManager.GenerateChangePhoneNumberTokenAsync(generateTokenPhoneDTO.UserId, generateTokenPhoneDTO.Phone);

                await UserManager.SendEmailAsync(generateTokenPhoneDTO.UserId, "Confirmação de Telefone!", string.Format("Olá, seu código para confirmar seu telefone é: {0}", token));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> ObterAutenticacaoDoisFatoresAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new ApplicationException("Usuário não encontrado!");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user.Id);

            return userFactors.ToList();
        }

        public async Task<ReturnVerifyCodeDTO> VerificarCodigoAsync(VerifyCodeDTO verifiyCode)
        {
            try
            {
                // Verificar token recebido!
                ApplicationUser user = await UserManager.FindByIdAsync(verifiyCode.UserId);
                if (user == null)
                {
                    throw new ApplicationException("Usuário não encontrado!");
                }

                bool token = await UserManager.VerifyTwoFactorTokenAsync(verifiyCode.UserId, verifiyCode.Provider, verifiyCode.Code);

                if (!token)
                {
                    throw new ApplicationException("Código inválido!");
                }

                return new ReturnVerifyCodeDTO
                {
                    ReturnUrl = verifiyCode.ReturnUrl
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoEmailAsync(ConfirmEmailCodeDTO confirmEmailCodeDTO)
        {
            try
            {
                var identityResult = await UserManager.ConfirmEmailAsync(confirmEmailCodeDTO.UserId, StringHelper.Base64ForUrlDecode(confirmEmailCodeDTO.Code));
                var result = new IdentityResultCodeDTO
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoTelefoneAsync(ConfirmPhoneCodeDTO confirmPhoneCodeDTO)
        {
            try
            {
                var identityResult = await UserManager.ChangePhoneNumberAsync(confirmPhoneCodeDTO.UserId, confirmPhoneCodeDTO.Phone, confirmPhoneCodeDTO.Code);
                var result = new IdentityResultCodeDTO
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            RoleManager.Dispose();
            SignInManager.Dispose();
            UserManager.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
