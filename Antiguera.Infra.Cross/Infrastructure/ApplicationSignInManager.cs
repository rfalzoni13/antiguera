using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Antiguera.Infra.Cross.Infrastructure
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            :base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> option, IOwinContext context)
        {
            var manager = context.GetUserManager<ApplicationUserManager>();

            var sign = new ApplicationSignInManager(manager, context.Authentication);

            return sign;
        }
    }
}
