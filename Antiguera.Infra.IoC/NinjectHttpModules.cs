using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Interfaces.Base;
using Antiguera.Aplicacao.Servicos;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Base;
using Antiguera.Infra.Data.Repositorios;
using Antiguera.Infra.Data.Repositorios.Base;
using Antiguera.Servicos;
using Antiguera.Servicos.Base;
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
                Kernel.Bind<IUsuarioRepositorio>().To<UsuarioRepositorio>();
                Kernel.Bind<IAcessoRepositorio>().To<AcessoRepositorio>();
                Kernel.Bind<IJogoRepositorio>().To<JogoRepositorio>();
                Kernel.Bind<IProgramaRepositorio>().To<ProgramaRepositorio>();
                Kernel.Bind<IEmuladorRepositorio>().To<EmuladorRepositorio>();
                Kernel.Bind<IRomRepositorio>().To<RomRepositorio>();

                Kernel.Bind(typeof(IServicoBase<>)).To(typeof(ServicoBase<>));
                Kernel.Bind<IUsuarioServico>().To<UsuarioServico>();
                Kernel.Bind<IAcessoServico>().To<AcessoServico>();
                Kernel.Bind<IJogoServico>().To<JogoServico>();
                Kernel.Bind<IProgramaServico>().To<ProgramaServico>();
                Kernel.Bind<IEmuladorServico>().To<EmuladorServico>();
                Kernel.Bind<IRomServico>().To<RomServico>();

                Kernel.Bind(typeof(IAppServicoBase<>)).To(typeof(AppServicoBase<>));
                Kernel.Bind<IUsuarioAppServico>().To<UsuarioAppServico>();
                Kernel.Bind<IAcessoAppServico>().To<AcessoAppServico>();
                Kernel.Bind<IJogoAppServico>().To<JogoAppServico>();
                Kernel.Bind<IProgramaAppServico>().To<ProgramaAppServico>();
                Kernel.Bind<IEmuladorAppServico>().To<EmuladorAppServico>();
                Kernel.Bind<IRomAppServico>().To<RomAppServico>();
            }
        }
    }
}
