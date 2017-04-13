namespace DDDLite.WebApi.Mvc
{
    using System.Threading.Tasks;
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    using Validation;


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

            if (error is ApiException)
            {
                var ex = (ApiException)error;
                context.Result = new ErrorMessageResult(ex.StatusCode, ex.ErrorModel);
            }
            else if (error is AuthorizedException)
            {
                context.Result = new ErrorMessageResult(((AuthorizedException)error).Status, new { message = error.Message });
            }
            else
            {
                context.Result = new ErrorMessageResult(500, new { message = error.Message });
            }
        }
    }
}