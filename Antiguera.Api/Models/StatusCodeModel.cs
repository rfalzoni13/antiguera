using System.Net;

namespace Antiguera.Api.Models
{
    public class StatusCodeModel
    {
        public virtual HttpStatusCode Status { get; set; }

        public string Message { get; set; }

        public string[] ErrorsResult { get; set; }
    }
}