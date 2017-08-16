namespace DDDLite.WebApi.Exception
{
    using System;
    using DDDLite.Exception;
    using DDDLite.WebApi.Models;

    public class WebApiException : CoreException
    {
        private readonly int statusCode;
        private readonly ErrorData errorData;

        public WebApiException(int statusCode, ErrorData errorData)
        {
            this.statusCode = statusCode;
            this.errorData = errorData;
        }

        public WebApiException(int statusCode, Exception ex) : this(statusCode, new ErrorData
        {
            Code = ex?.GetType().Name,
            Message = ex.Message,
            Target = GetTarget(ex)
        })
        {
        }

        public ErrorData GetError() => this.errorData;

        public int GetStatusCode() => this.statusCode;

        internal static string GetTarget(Exception ex)
        {
            if (ex is BadArgumentException)
            {
                return (ex as BadArgumentException).Argument;
            }

            return null;
        }
    }
}