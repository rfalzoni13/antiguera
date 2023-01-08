using Antiguera.Dominio.DTO.Identity;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Antiguera.Dominio.Interfaces.Servicos
{
    public interface IAccountServico
    {
        Task Adicionar(ApplicationUserRegisterDTO register);
        Task<IdentityResultCodeDTO> AdicionarLoginExterno(string userId, string externalAccessToken);
        Task<IdentityResultCodeDTO> AlterarSenha(ChangePasswordBindingDTO changePasswordBindingDTO);
        Task Apagar(ApplicationUserRegisterDTO register);
        Task Atualizar(ApplicationUserRegisterDTO register);
        Task EnviarCodigo(SendCodeDTO sendCode);
        Task EnviarCodigoConfirmacaoEmail(GenerateTokenEmailDTO generateTokenEmailDTO);
        Task EnviarCodigoConfirmacaoTelefone(GenerateTokenPhoneDTO generateTokenPhoneDTO);
        Task EnviarEmailRecuperacaoSenha(ConfirmEmailCodeDTO confirmEmailCodeDTO);
        Task<ConfirmEmailCodeDTO> GerarTokenRecuperacaoSenha(string email);
        Task<List<string>> ObterAutenticacaoDoisFatores(string email);
        Task<IdentityResultCodeDTO> RecuperarSenha(ResetPasswordDTO resetPasswordDTO);
        Task<IdentityResultCodeDTO> RemoverLoginExterno(string userId, string loginProvider, string loginKey);
        Task<ReturnCodeStatusDTO> VerificarCodigo(VerifyCodeDTO verifiyCode);
        Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoEmail(ConfirmEmailCodeDTO confirmEmailCodeDTO);
        Task<IdentityResultCodeDTO> VerificarCodigoConfirmacaoTelefone(ConfirmPhoneCodeDTO confirmPhoneCodeDTO);
    }
}
