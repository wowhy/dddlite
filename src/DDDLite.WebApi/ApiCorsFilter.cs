namespace DDDLite.WebApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ApiCorsFilter : IAuthorizationFilter, IAsyncAuthorizationFilter, IOrderedFilter
    {
        public int Order => int.MinValue;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            if (!request.Path.StartsWithSegments("/api"))
            {
                return;
            }

            var response = context.HttpContext.Response;
            response.Headers.Add(CorsConstants.AccessControlAllowOrigin, "*");
            response.Headers.Add(CorsConstants.AccessControlAllowMethods, "GET,POST,PUT,PATCH,DELETE,HEAD,COPY,LOCK,UNLOCK,OPTIONS");
            response.Headers.Add(CorsConstants.AccessControlAllowHeaders, "Authorization,X-Requested-With,Request-Id,X-Request-Id,Cache-Control,Content-Type,Date,Expires,Last-Modified,If-Match,If-Modified-Since,Range,User-Agent");
            response.Headers.Add(CorsConstants.AccessControlAllowCredentials, "true");

            if (string.CompareOrdinal(request.Method, "OPTIONS") == 0)
            {
                Console.WriteLine("OPTIONS: {0}", request.Path);
                context.Result = new StatusCodeResult(200);
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            this.OnAuthorization(context);
        }
    }
}