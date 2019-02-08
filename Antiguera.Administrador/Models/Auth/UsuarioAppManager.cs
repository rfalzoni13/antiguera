using Antiguera.Administrador.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Antiguera.Administrador.Models.Auth
{
    public class UsuarioAppManager : UserManager<UserModel>
    {
        public UsuarioAppManager(IUserStore<UserModel> store)
            : base(store)
        {
        }

        public static UsuarioAppManager Create(IdentityFactoryOptions<UsuarioAppManager> options, IOwinContext context)
        {
            var appcontext = context.Get<Contexto>();

            var usuarioManager = new UsuarioAppManager(new UserStore<UserModel>(appcontext));

            return usuarioManager;
        }
    }
}