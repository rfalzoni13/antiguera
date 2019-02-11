using Antiguera.WebApi.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Antiguera.WebApi
{
    public class AccessProviderToken : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {            
            try
            {
                DbContext db = new DbContext("Antiguera");

                var usuario = db.Database.SqlQuery<UsuarioModel>("exec Antiguera_Login @Login ", new SqlParameter("@Login", context.UserName)).FirstOrDefault();

                if(usuario != null)
                {
                    if (BCrypt.CheckPassword(context.Password, usuario.Senha))
                    {
                        usuario.Acesso = db.Database.SqlQuery<AcessoModel>("SELECT Nome FROM Acesso WHERE Id=@Id ", new SqlParameter("@Id", usuario.AcessoId)).FirstOrDefault();

                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Acesso.Nome));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Password error: ", "Senha inválida!");
                    }
                }
                else
                {
                    context.SetError("Not Found: ", "Nenhum usuário encontrado!");
                }
            }

            catch(SqlException e)
            {
                context.SetError("Conection error: ", e.Message);
            }

            catch (Exception e)
            {
                context.SetError("Authentication error: " + e.Message);
            }
            return Task.FromResult<object>(0);
        }
    }
}