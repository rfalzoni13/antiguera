using Ninject;
using Ninject.Modules;
using System.Reflection;
using System.Web.Http;

namespace Antiguera.Infra.IoC
{
    public class NinjectHttpContainer
    {
        private static NinjectHttpResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectHttpResolver(modules);
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        public static void RegisterAssembly()
        {
            _resolver = new NinjectHttpResolver(Assembly.GetExecutingAssembly());
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        public static T Resolve<T>()
        {
            return _resolver.kernel.Get<T>();
        }
    }
}
