using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using Antiguera.Servicos.Servicos.Identity;
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

            //Serviços
            container.RegisterType<AcessoServico>();
            container.RegisterType<AccountServico>();
            container.RegisterType<IdentityUtilityServico>();
            container.RegisterType<UsuarioServico>();

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
