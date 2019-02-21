using Antiguera.Administrador.AutoMapper;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Antiguera.Administrador
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Código que é executado na inicialização do aplicativo
           FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
           RouteConfig.RegisterRoutes(RouteTable.Routes);
           BundleConfig.RegisterBundles(BundleTable.Bundles);
           AutoMapperConfig.RegisterMappings();
        }
    }
}