using System.Web.Http;
using Unity.AspNet.WebApi;

namespace Antiguera.Infra.IoC
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = UnityModule.LoadModules();

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
