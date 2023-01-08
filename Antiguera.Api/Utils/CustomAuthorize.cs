using Antiguera.Api.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Antiguera.Api.Utils
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            StatusCodeModel stats = new StatusCodeModel();
            stats.Status = HttpStatusCode.Unauthorized;
            stats.Message = "Você não esta autorizado a acessar este conteúdo!";

            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new ObjectContent(stats.GetType(), stats, new JsonMediaTypeFormatter())
            };
        }
    }
}