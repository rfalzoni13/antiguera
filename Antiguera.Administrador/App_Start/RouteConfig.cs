using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Antiguera.Administrador
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //MapearRotasJavascript(routes);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        //private static void MapearRotasJavascript(RouteCollection routes)
        //{
        //    routes.MapRoute("Rotas", "Scripts/rotas.js", new { controller = "Resource", action = "Rotas" });
        //}
    }
}
