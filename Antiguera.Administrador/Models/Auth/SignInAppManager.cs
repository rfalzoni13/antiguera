using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Antiguera.Administrador.Models.Auth
{
    public class SignInAppManager : SignInManager<UserModel, string>
    {
        public SignInAppManager(UsuarioAppManager userManager, IAuthenticationManager authenticationManager)
            :base(userManager, authenticationManager)
        {
        }

        public static SignInAppManager Create(IdentityFactoryOptions<SignInAppManager> option, IOwinContext context)
        {
            var manager = context.GetUserManager<UsuarioAppManager>();

            var sign = new SignInAppManager(manager, context.Authentication);

            return sign;
        }
    }
}