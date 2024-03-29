[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Antiguera.Administrador.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Antiguera.Administrador.App_Start.NinjectWebCommon), "Stop")]

namespace Antiguera.Administrador.App_Start
{
    using Antiguera.Administrador.Clients;
    using Antiguera.Administrador.Clients.Base;
    using Antiguera.Administrador.Clients.Interface;
    using Antiguera.Administrador.Filters;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using System;
    using System.Web;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(IClientBase<,>), typeof(ClientBase<,>));
            kernel.Bind<IUsuarioClient>().To<UsuarioClient>().InRequestScope();

            kernel.Bind<AccountClient>().ToSelf().InRequestScope();

            kernel.BindFilter<DashBoardActionAttribute>(System.Web.Mvc.FilterScope.Global, 1).InRequestScope();
        }
    }
}