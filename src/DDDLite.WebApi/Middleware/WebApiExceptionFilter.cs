namespace DDDLite.WebApi.Middleware
{
    using System;
    using DDDLite.WebApi.Exception;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class WebApiExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger logger;

        public WebApiExceptionFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<WebApiExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            logger?.LogError(new EventId(), context.Exception, "An unhandled exception occurred during the request");

            var exception = WebApiExceptionFactory.GetException(context.Exception);

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(exception.GetError())
            {
                StatusCode = exception.GetStatusCode()
            };
        }
    }
}