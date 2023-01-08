using System.Net;

namespace Antiguera.Administrador.Models
{
    public class StatusCodeModel
    {
        public virtual HttpStatusCode Status { get; set; }

        public string Message { get; set; }
    }
}