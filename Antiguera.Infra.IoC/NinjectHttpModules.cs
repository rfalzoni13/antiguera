using Antiguera.Dominio.Helpers;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Infra.Data.Repositorios;
using Antiguera.Infra.Data.Repositorios.Base;
using Antiguera.Servicos.Servicos;
using Ninject.Modules;

namespace Antiguera.Infra.IoC
{
    public class NinjectHttpModules
    {
        //Return Lists of Modules in the Application
        public static NinjectModule[] Modules
        {
            get
            {
                return new[] { new MainModule() };
            }
        }

        //Main Module For Application
        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(typeof(IRepositorioBase<>)).To(typeof(RepositorioBase<>));
                Kernel.Bind<IAcessoRepositorio>().To<AcessoRepositorio>();                
                Kernel.Bind<IEmuladorRepositorio>().To<EmuladorRepositorio>();
                Kernel.Bind<IHistoricoRepositorio>().To<HistoricoRepositorio>();
                Kernel.Bind<IJogoRepositorio>().To<JogoRepositorio>();
                Kernel.Bind<IProgramaRepositorio>().To<ProgramaRepositorio>();
                Kernel.Bind<IRomRepositorio>().To<RomRepositorio>();
                Kernel.Bind<IUsuarioRepositorio>().To<UsuarioRepositorio>();

                Kernel.Bind<IAcessoServico>().To<AcessoServico>();
                Kernel.Bind<IEmuladorServico>().To<EmuladorServico>();
                Kernel.Bind<IJogoServico>().To<JogoServico>();
                Kernel.Bind<IProgramaServico>().To<ProgramaServico>();
                Kernel.Bind<IRomServico>().To<RomServico>();
                Kernel.Bind<IUsuarioServico>().To<UsuarioServico>();

                Kernel.Bind(typeof(IConvertHelper<,>)).To(typeof(ConvertHelper<,>));
                Kernel.Bind<IUnitOfWork, UnitOfWork>();
            }
        }
    }
}
