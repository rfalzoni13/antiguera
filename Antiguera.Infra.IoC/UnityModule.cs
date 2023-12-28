using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios;
using Antiguera.Infra.Data.Repositorios.Base;
using Antiguera.Servicos.Servicos;
using Antiguera.Utils.Helpers;
using System.Data.Entity;
using Unity;

namespace Antiguera.Infra.IoC
{
    public class UnityModule
    {
        public static UnityContainer LoadModules()
        {
            var container = new UnityContainer();

            //Repositórios
            container.RegisterType(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            container.RegisterType<IEmuladorRepositorio, EmuladorRepositorio>();
            container.RegisterType<IHistoricoRepositorio, HistoricoRepositorio>();
            container.RegisterType<IJogoRepositorio, JogoRepositorio>();
            container.RegisterType<IProgramaRepositorio, ProgramaRepositorio>();
            container.RegisterType<IRomRepositorio, RomRepositorio>();

            //Serviços
            container.RegisterType(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            container.RegisterType<IAcessoServico, AcessoServico>();
            container.RegisterType<IEmuladorServico, EmuladorServico>();
            container.RegisterType<IJogoServico, JogoServico>();
            container.RegisterType<IProgramaServico, ProgramaServico>();
            container.RegisterType<IRomServico, RomServico>();
            container.RegisterType<IUsuarioServico, UsuarioServico>();
            container.RegisterType<IAccountServico, AccountServico>();

            //Complementares
            container.RegisterType(typeof(IConvertHelper<,>), typeof(ConvertHelper<,>));
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            //Context
            container.RegisterType<DbContext, AntigueraContexto>();
            container.RegisterType<DbContext, ApplicationDbContext>();

            return container;
        }        
    }
}
