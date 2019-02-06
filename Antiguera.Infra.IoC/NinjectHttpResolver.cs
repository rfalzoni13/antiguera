using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dependencies;

namespace Antiguera.Infra.IoC
{
    public class NinjectHttpResolver : IDependencyResolver, IDependencyScope
    {
        public IKernel kernel { get; private set; }
        public NinjectHttpResolver(params NinjectModule[] modules)
        {
            kernel = new StandardKernel(modules);
        }

        public NinjectHttpResolver(Assembly assembly)
        {
            kernel = new StandardKernel();
            kernel.Load(assembly);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}
