using Antiguera.Api.Utils;
using Antiguera.Infra.IoC;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Web.Http;

namespace Antiguera.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web

            // Configuração e serviços de API Web
            // Configure a API Web para usar somente a autenticação de token de portador.
            config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new CustomAuthorizeAttribute());
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            UnityConfig.Register(config);

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
