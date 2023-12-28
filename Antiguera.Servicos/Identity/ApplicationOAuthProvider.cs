using Antiguera.Infra.Data.Identity;
using Antiguera.Servicos.Identity;
using Antiguera.Utils.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.Servicos.Identity
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                //var signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
                var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                //var email = new EmailAddressAttribute();

                ApplicationUser user = await userManager.FindByEmailAsync(context.UserName);
                    //?? await userManager.FindByNameAsync(context.UserName);

                bool login = await userManager.CheckPasswordAsync(user, context.Password);

                if (!login)
                {
                    context.SetError("invalid_grant", "Usuário ou senha incorretos!");
                    return;
                }

                //bool twoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user.Id);
                //if (twoFactorEnabled)
                //{
                //    var code = await userManager.GenerateTwoFactorTokenAsync(user.Id, "PhoneCode");
                //    IdentityResult notificationResult = await userManager.NotifyTwoFactorTokenAsync(user.Id, "PhoneCode", code);
                //    if (!notificationResult.Succeeded)
                //    {
                //        //you can add your own validation here
                //        context.SetError("invalid_grant", "Failed to send OTP");
                //    }
                //}

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }

            catch (SqlException e)
            {
                context.SetError("Conection error: ", ExceptionHelper.CatchMessageFromException(e));
            }

            catch (Exception e)
            {
                context.SetError("Authentication error: " + ExceptionHelper.CatchMessageFromException(e));
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {  
            var identity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(identity, context.Ticket.Properties);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public static async Task ExternalLogin(IOwinContext context, IPrincipal user, string provider)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(user.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                throw new Exception("Login não encontrado!");
            }

            if (externalLogin.LoginProvider != provider)
            {
                Logout(context, DefaultAuthenticationTypes.ExternalCookie);
                throw new ApplicationException("Usuário não cadastrado com provedor de login");
            }

            ApplicationUser applicationUser = await userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = applicationUser != null;

            if (hasRegistered)
            {
                Logout(context, DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await applicationUser.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await applicationUser.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(applicationUser);
                context.Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity ClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                context.Authentication.SignIn(ClaimIdentity);
            }
        }

        public static async Task<IdentityResult> RegisterExternal(IOwinContext context, string userName, string email)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var info = await context.Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new Exception("Informações não encontradas!");
            }

            var user = new ApplicationUser() { UserName = userName, Email = email };

            IdentityResult result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            result = await userManager.AddLoginAsync(user.Id, info.Login);
            
            return result;
        }

        public static void Logout(IOwinContext context, string authenticationType)
        {
            var authentication = context.Authentication;

            authentication.SignOut(authenticationType);
        }

        public static IEnumerable<AuthenticationDescription> ObterAuthenticationTypes(IOwinContext context)
        {
            return context.Authentication.GetExternalAuthenticationTypes();
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userId", user.Id },
                { "roleId", user.Roles.Select(x => x.RoleId).FirstOrDefault() }
            };
            return new AuthenticationProperties(data);
        }
    }
}