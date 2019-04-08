using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Antiguera.WebApi.Authorization
{
    public class AccessProviderToken : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                    var roles = userManager.GetRoles(user.Id).ToList();
                    if (roles != null && roles.Count > 0)
                    {
                        foreach (var role in roles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }

                    context.Validated(identity);
                }
                else
                {
                    context.SetError("Not Found: ", "Nenhum usuário encontrado!");
                }
            }

            catch (SqlException e)
            {
                context.SetError("Conection error: ", e.Message);
            }

            catch (Exception e)
            {
                context.SetError("Authentication error: " + e.Message);
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {  
            var identity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(identity, context.Ticket.Properties);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}