namespace DDDLite.WebApi
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class ErrorMessageResult : BadRequestObjectResult
    {
        public ErrorMessageResult(int statusCode, object message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
