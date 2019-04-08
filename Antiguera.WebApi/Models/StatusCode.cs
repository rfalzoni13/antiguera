﻿using System.Net;

namespace Antiguera.WebApi.Models
{
    public class StatusCode
    {
        public virtual HttpStatusCode Status { get; set; }

        public string Message { get; set; }
    }
}