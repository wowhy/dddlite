namespace DDDLite.WebApi
{
    using System.Threading.Tasks;
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    using Commands.Validation;


    public class ApiExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            await this.DoFilter(context);
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            return this.DoFilter(context);
        }

        private async Task DoFilter(ExceptionContext context)
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
                await response.WriteAsync(
                    JsonConvert.SerializeObject(new ErrorMessage(error.Message, ((ValidationException)error).Details)), 
                    Encoding.UTF8);
            }
            else
            {
                response.StatusCode = 500;
                await response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorMessage(
                            error.Message, 
                            error.InnerException == null ? new string[0] : new string[] { error.InnerException.Message })
                        ), 
                    Encoding.UTF8);
            }
        }
    }
}