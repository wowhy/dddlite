namespace DDDLite.WebApi
{
    using System.Threading.Tasks;
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    using Commands.Validation;


    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var error = context.Exception;
            var response = context.HttpContext.Response;

            if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                return;
            }

            // context.ExceptionHandled = true;

            if (error is ValidationException)
            {
                context.Result = new ErrorMessageResult(400, new ErrorMessage(error.Message, ((ValidationException)error).Details));
            }
            else if (error is AuthorizedException)
            {
                context.Result = new ErrorMessageResult(((AuthorizedException)error).Status, new ErrorMessage(error.Message));
            }
            else
            {
                context.Result = new ErrorMessageResult(500, new ErrorMessage(error.Message, error.InnerException == null ? new string[0] : new string[] { error.InnerException.Message }));
            }
        }
    }
}