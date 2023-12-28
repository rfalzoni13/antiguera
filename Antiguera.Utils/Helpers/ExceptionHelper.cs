using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Antiguera.Utils.Helpers
{
    public class ExceptionHelper
    {
        public static string CatchMessageFromException(Exception ex)
        {
            if(ex.InnerException != null)
            {
                return ex.InnerException.Message;
            }

            switch(ex.GetType())
            {
                case Type applicationExceptionType when applicationExceptionType == typeof(ApplicationException):
                    return ex.Message;

                case Type sqlServerException when sqlServerException == typeof(SqlException):
#if !DEBUG
                    return "Erro de comunicação com o servidor! Por favor, tente novamente mais tarde!";
#else
                    return ex.Message;
#endif
                case Type taskCancelledException when taskCancelledException == typeof(TaskCanceledException):
                    return "Erro de comunicação com o servidor! Por favor, tente novamente mais tarde!";

                default:
#if !DEBUG
                    return "Ocorreu um erro ao processar sua solicitação!";
#else
                    return ex.Message;
#endif
            }
        }
    }
}
