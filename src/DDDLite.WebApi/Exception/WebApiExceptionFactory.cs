namespace DDDLite.WebApi.Exception
{
    using System;
    using DDDLite.Exception;

    public class WebApiExceptionFactory
    {
        public static WebApiException GetException(Exception ex)
        {
            var statusCode = 500;

            if (ex is CoreException)
            {
                statusCode = 400;

                if (ex is AggregateNotFoundException)
                {
                    statusCode = 404;
                }
            }

            return new WebApiException(statusCode, ex);
        }
    }
}