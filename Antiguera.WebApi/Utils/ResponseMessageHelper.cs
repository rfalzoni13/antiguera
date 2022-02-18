using Antiguera.WebApi.Models;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.WebApi.Utils
{
    public static class ResponseMessageHelper
    {
        public static HttpResponseMessage RetornoExceptionNaoEncontrado(HttpResponseException ex, HttpRequestMessage request, Logger logger, string action, string message)
        {
            logger.Warn(action + " - Error: " + ex);

            StatusCode status = new StatusCode
            {
                Status = ex.Response.StatusCode,
                Message = "Nenhum registro encontrado!"
            };

            logger.Info(action + " - Finalizado");
            return request.CreateResponse(HttpStatusCode.NotFound, status);
        }

        public static HttpResponseMessage RetornoExceptionErroInterno(Exception ex, HttpRequestMessage request, Logger logger, string action)
        {
            logger.Error(action + " - Error: " + ex);

            StatusCode status = new StatusCode
            {
                Status = HttpStatusCode.InternalServerError,
                Message = ex.Message
            };

            logger.Info(action + " - Finalizado");
            return request.CreateResponse(HttpStatusCode.InternalServerError, status);
        }

        public static HttpResponseMessage RetornoRequisicaoInvalida(HttpRequestMessage request, Logger logger, string action, string message)
        {
            logger.Warn(action + " - " + message);

            StatusCode status = new StatusCode
            {
                Status = HttpStatusCode.BadRequest,
                Message = message
            };

            logger.Info(action + " - Finalizado");
            return request.CreateResponse(HttpStatusCode.BadRequest, status);
        }
    }
}