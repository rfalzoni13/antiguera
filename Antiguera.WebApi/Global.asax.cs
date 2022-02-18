using Antiguera.Infra.IoC;
using System.Web;
using System.Web.Http;

namespace Antiguera.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules);
        }
    }
}
