namespace DDDLite.WebApi
{
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    using Commands.Validation;

    public class ApiExceptionFilter : IExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            var error = context.Exception;
            var response = context.HttpContext.Response;

            if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                return;
            }

            response.Clear();
            response.ContentType = "application/json";
            context.ExceptionHandled = true;

            if (error is ValidationException)
            {
                response.StatusCode = 400;
                await response.WriteAsync(JsonConvert.SerializeObject(
                    new
                    {
                        message = error.Message,
                        details = ((ValidationException)error).Details
                    }), Encoding.UTF8);
            }
            else
            {
                response.StatusCode = 500;
                await response.WriteAsync(JsonConvert.SerializeObject(
                    new
                    {
                        message = error.Message,
                        details = error.InnerException == null ? new string[0] : new string[] { error.InnerException.Message }
                    }), Encoding.UTF8);
            }
        }
    }
}