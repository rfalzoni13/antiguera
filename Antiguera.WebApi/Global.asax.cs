using Antiguera.Infra.IoC;
using Antiguera.WebApi.AutoMapper;
using System.Web.Http;

namespace Antiguera.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.RegisterMappings();
            NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules);
        }
    }
}
