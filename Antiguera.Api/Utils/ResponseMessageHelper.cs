using Antiguera.Api.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Antiguera.Api.Utils
{
    public static class ResponseMessageHelper
    {
        public static HttpResponseMessage RetornoExceptionNaoEncontrado(HttpResponseException ex, HttpRequestMessage request, Logger logger, string action, string message)
        {
            logger.Warn(action + " - Error: " + ex);

            StatusCodeModel status = new StatusCodeModel
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

            StatusCodeModel status = new StatusCodeModel
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

            StatusCodeModel status = new StatusCodeModel
            {
                Status = HttpStatusCode.BadRequest,
                Message = message
            };

            logger.Info(action + " - Finalizado");
            return request.CreateResponse(HttpStatusCode.BadRequest, status);
        }

        public static HttpResponseMessage RetornoErrorResult(HttpRequestMessage request, Logger logger, string action, IEnumerable<string> errors)
        {
            string message = string.Empty;

            int i = 0;

            string[] errorsArray = errors.ToArray();

            do
            {
                message += errorsArray[i] + " ";
                i++;
            }
            while (i < errorsArray.Count());

            logger.Warn(action + " - " + message);

            StatusCodeModel status = new StatusCodeModel
            {
                Status = HttpStatusCode.BadRequest,
                ErrorsResult = errorsArray
            };

            logger.Info(action + " - Finalizado");
            return request.CreateResponse(HttpStatusCode.BadRequest, status);
        }
    }
}