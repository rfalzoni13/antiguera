using Antiguera.Servicos.Servicos.Identity;
using Owin;

namespace Antiguera.Administrador
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AppBuilderConfiguration.ConfigureAuth(app);
        }
    }
}
