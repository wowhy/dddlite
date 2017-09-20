namespace DDDLite.WebApi.Middleware
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using DDDLite.WebApi.Exception;
    using DDDLite.WebApi.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class WebApiExceptionMiddleware
    {
        private readonly static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly RequestDelegate next;

        private readonly ILogger logger;

        public WebApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFacotry)
        {
            this.next = next;
            this.logger = loggerFacotry.CreateLogger<WebApiExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (Exception ex)
            {
                await WriteExceptionResponseAsync(context, ex);
            }
        }

        private async Task WriteExceptionResponseAsync(HttpContext context, Exception ex)
        {
            logger?.LogError(new EventId(), ex, "An unhandled exception occurred during the request");

            var exception = WebApiExceptionFactory.GetException(ex);
            var message = JsonConvert.SerializeObject(new ResponseError(exception.GetError()), settings);

            context.Response.StatusCode = exception.GetStatusCode();
            context.Response.ContentLength = Encoding.UTF8.GetBytes(message).Length;
            context.Response.ContentType = "application/json; charset=utf-8";

            await context.Response.WriteAsync(message, Encoding.UTF8);
        }
    }
}