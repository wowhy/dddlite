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
            }

            return new WebApiException(statusCode, ex);
        }
    }
}