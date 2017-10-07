namespace DDDLite.WebApi.Exception
{
    using System;
    using DDDLite.Exception;
    using DDDLite.WebApi.Models;

    public class WebApiExceptionFactory
    {
        public static WebApiException GetException(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var statusCode = GetStatusCode(ex);
            var errorData = new ErrorData
            {
                Code = ex.GetType().Name,
                Message = ex.Message,
                Target = GetTarget(ex)
            };

            return new WebApiException(statusCode, errorData);
        }

        private static int GetStatusCode(Exception ex)
        {
            if (ex is CoreException)
            {
                if (ex is AggregateRootNotFoundException)
                {
                    return 404;
                }
                else
                {
                    return 400;
                }
            }

            return 500;
        }

        private static string GetTarget(Exception ex)
        {
            if (ex is BadArgumentException)
            {
                return (ex as BadArgumentException).Argument;
            }

            if (ex is AggregateRootException) 
            {
                return (ex as AggregateRootException).Id.ToString();
            }

            return null;
        }
    }
}