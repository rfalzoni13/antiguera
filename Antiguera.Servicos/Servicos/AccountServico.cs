using Antiguera.Dominio.DTO;
using Antiguera.Dominio.DTO.Identity;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.Identity;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class AccountServico : IAccountServico, IDisposable
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private readonly IUnitOfWork _unitOfWork;

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

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public AccountServico(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Adicionar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Adicionar usuário
                    var user = UserManager.FindByNameAsync(register.Login).Result;

                    if (user != null)
                    {
                        throw new ApplicationException("Já existe um usuário com estas informações!");
                    }

                    user = new ApplicationUser()
                    {
                        FirstName = register.Nome.Split(' ').FirstOrDefault(),
                        LastName = register.Nome.Split(' ').LastOrDefault(),
                        Email = register.Email,
                        PhoneNumber = register.Telefone,
                        UserName = register.Login,
                        Active = true,
                        Created = DateTime.Now
                    };

                    var result = await UserManager.CreateAsync(user, register.Senha);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao incluir usuário!");
                    }

                    //Adicionar acesso
                    int i = 0;

                    while (i < register.Acessos.Count())
                    {
                        var role = await RoleManager.FindByNameAsync(register.Acessos[i]);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        i++;

                        result = await UserManager.AddToRoleAsync(user.Id, role.Name);

                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao incluir perfil de acesso!");
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<IdentityResultCodeDTO> AdicionarLoginExterno(string userId, string externalAccessToken)
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

        public async Task<IdentityResultCodeDTO> AlterarSenha(ChangePasswordBindingDTO changePasswordBindingDTO)
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

        public async Task Apagar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(register.ID);

                    if (user == null)
                    {
                        throw new ArgumentNullException("Nenhum usuário encontrado!");
                    }

                    var result = UserManager.DeleteAsync(user).Result;

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao excluir usuário!");
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public async Task Atualizar(ApplicationUserRegisterDTO register)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Atualizar usuário
                    var user = await UserManager.FindByIdAsync(register.ID);

                    if (user == null)
                    {
                        throw new ApplicationException("Nenhum usuário encontrado!");
                    }

                    user.FirstName = register.Nome.Split(' ').FirstOrDefault();
                    user.LastName = register.Nome.Split(' ').LastOrDefault();
                    user.Email = register.Email;
                    user.PhotoPath = register.PathFoto;
                    user.PhoneNumber = register.Telefone;
                    user.Modified = DateTime.Now;

                    var result = await UserManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException("Erro ao atualizar usuário!");
                    }

                    //Atualizar acesso
                    int i = 0;

                    do
                    {
                        var role = await RoleManager.FindByNameAsync(register.ID);
                        if (role == null)
                        {
                            throw new ArgumentNullException("Nenhum registro de permissão de acesso encontrado!");
                        }

                        var roles = UserManager.GetRolesAsync(user.Id).Result;
                        if (roles == null || roles.Count() <= 0)
                        {
                            throw new ApplicationException("Erro ao cadastrar novo perfil de acesso!");
                        }

                        result = UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray()).Result;

                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao realizar manutenção de acesso!");
                        }

                        result = UserManager.AddToRoleAsync(user.Id, role.Name).Result;

                        if (!result.Succeeded)
                        {
                            throw new DbUpdateException("Erro ao atualizar acesso!");
                        }
                    }
                    while (i < register.Acessos.Count());

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public async Task EnviarCodigo(SendCodeDTO sendCode)
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

                await UserManager.NotifyTwoFactorTokenAsync(sendCode.UserId, sendCode.SelectedProvider, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnviarCodigoConfirmacaoEmail(GenerateTokenEmailDTO generateTokenEmailDTO)
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

        public async Task EnviarCodigoConfirmacaoTelefone(GenerateTokenPhoneDTO generateTokenPhoneDTO)
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

        public async Task EnviarEmailRecuperacaoSenha(ConfirmEmailCodeDTO confirmEmailCodeDTO)
        {
            await UserManager.SendEmailAsync(confirmEmailCodeDTO.UserId, "Redefinir senha", "Redefina sua senha, clicando <a href=\"" + confirmEmailCodeDTO.CallBackUrl + "\">aqui</a>");
        }

        public async Task<ConfirmEmailCodeDTO> GerarTokenRecuperacaoSenha(string email)
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

        public async Task<List<string>> ObterAutenticacaoDoisFatores(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new ApplicationException("Usuário não encontrado!");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user.Id);

            return userFactors.ToList();
        }

        public async Task<IdentityResultCodeDTO> RecuperarSenha(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await UserManager.FindByNameAsync(resetPasswordDTO.Email);
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

        public async Task<IdentityResultCodeDTO> RemoverLoginExterno(string userId, string loginProvider, string loginKey)
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

        public async Task<ReturnVerifyCodeDTO> VerificarCodigo(VerifyCodeDTO verifiyCode)
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

        public async Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoEmail(ConfirmEmailCodeDTO confirmEmailCodeDTO)
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

        public async Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoTelefone(ConfirmPhoneCodeDTO confirmPhoneCodeDTO)
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

        public void Dispose()
        {
            _userManager.Dispose();
            _userManager = null;
            _roleManager.Dispose();
            _roleManager = null;
            _signInManager.Dispose();
            _signInManager = null;

            GC.SuppressFinalize(this);
        }
    }
}
