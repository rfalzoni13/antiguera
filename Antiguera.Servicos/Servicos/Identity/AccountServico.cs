using Antiguera.Dominio.DTO.Identity;
using Antiguera.Servicos.IdentityConfiguration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.Servicos.Servicos.Identity
{
    public class AccountServico : IDisposable
    {
        #region Atributos
        private const string LocalLoginProvider = "Local";
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
        public IdentityResultCodeDTO AdicionarLoginExterno(string userId, string externalAccessToken)
        {
            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(externalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                throw new ApplicationException("Falha no login externo.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                throw new ApplicationException("O login externo já está associado a uma conta.");
            }

            IdentityResult result = UserManager.AddLogin(userId,
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }

        public IdentityResultCodeDTO AlterarSenha(ChangePasswordBindingDTO changePasswordBindingDTO)
        {
            var result = UserManager.ChangePassword(changePasswordBindingDTO.UserId, changePasswordBindingDTO.OldPassword,
                            changePasswordBindingDTO.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Erro ao alterar senha!");
            }

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }

        public IdentityResultCodeDTO RecuperarSenha(ResetPasswordDTO resetPasswordDTO)
        {
            var user = UserManager.FindByEmail(resetPasswordDTO.Email);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado!");
            }
            var identityResult = UserManager.ResetPassword(user.Id, resetPasswordDTO.Code, resetPasswordDTO.Password);

            var result = new IdentityResultCodeDTO
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors
            };

            return result;
        }

        public void EnviarEmailRecuperacaoSenha(ConfirmEmailCodeDTO confirmEmailCodeDTO)
        {
            UserManager.SendEmail(confirmEmailCodeDTO.UserId, "Redefinir senha", "Redefina sua senha, clicando <a href=\"" + confirmEmailCodeDTO.CallBackUrl + "\">aqui</a>");
        }

        public ConfirmEmailCodeDTO GerarTokenRecuperacaoSenha(string email)
        {
            var user = UserManager.FindByEmail(email);
            if (user == null || !(UserManager.IsEmailConfirmed(user.Id)))
            {
                // Não revelar que o usuário não existe ou não está confirmado
                throw new Exception("Usuário/Email não existente ou não confirmado!");
            }

            // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
            // Enviar um email com este link
            string code = UserManager.GeneratePasswordResetToken(user.Id);

            return new ConfirmEmailCodeDTO
            {
                UserId = user.Id,
                Code = code
            };
        }

        public IdentityResultCodeDTO RemoverLoginExterno(string userId, string loginProvider, string loginKey)
        {
            IdentityResult result;

            if (loginProvider == LocalLoginProvider)
            {
                result = UserManager.RemovePassword(userId);
            }
            else
            {
                result = UserManager.RemoveLogin(userId, new UserLoginInfo(loginProvider, loginKey));
            }

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }
        #endregion

        #region Async Services
        public async Task<IdentityResultCodeDTO> AdicionarLoginExternoAsync(string userId, string externalAccessToken)
        {
            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(externalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                throw new ApplicationException("Falha no login externo.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                throw new ApplicationException("O login externo já está associado a uma conta.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(userId,
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }

        public async Task<IdentityResultCodeDTO> AlterarSenhaAsync(ChangePasswordBindingDTO changePasswordBindingDTO)
        {
            var result = await UserManager.ChangePasswordAsync(changePasswordBindingDTO.UserId, changePasswordBindingDTO.OldPassword,
                            changePasswordBindingDTO.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Erro ao alterar senha!");
            }

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }

        public async Task<IdentityResultCodeDTO> RecuperarSenhaAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await UserManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado!");
            }
            var identityResult = await UserManager.ResetPasswordAsync(user.Id, resetPasswordDTO.Code, resetPasswordDTO.Password);

            var result = new IdentityResultCodeDTO
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors
            };

            return result;
        }

        public async Task EnviarEmailRecuperacaoSenhaAsync(ConfirmEmailCodeDTO confirmEmailCodeDTO)
        {
            await UserManager.SendEmailAsync(confirmEmailCodeDTO.UserId, "Redefinir senha", "Redefina sua senha, clicando <a href=\"" + confirmEmailCodeDTO.CallBackUrl + "\">aqui</a>");
        }

        public async Task<ConfirmEmailCodeDTO> GerarTokenRecuperacaoSenhaAsync(string email)
        {
            var user = await UserManager.FindByNameAsync(email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Não revelar que o usuário não existe ou não está confirmado
                throw new Exception("Usuário/Email não existente ou não confirmado!");
            }

            // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
            // Enviar um email com este link
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            return new ConfirmEmailCodeDTO
            {
                UserId = user.Id,
                Code = code
            };
        }

        public async Task<IdentityResultCodeDTO> RemoverLoginExternoAsync(string userId, string loginProvider, string loginKey)
        {
            IdentityResult result;

            if (loginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(userId);
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, loginKey));
            }

            return new IdentityResultCodeDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
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
