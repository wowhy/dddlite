namespace DDDLite.WebApi.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ConcurrencyVersionFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult)
            {
                var result = (OkObjectResult)context.Result;
                if (result.Value is IConcurrencyVersion)
                {
                    context.HttpContext.Response.Headers["ETag"] = ((IConcurrencyVersion)result.Value).RowVersion.ToString();
                }
            }
            else if (context.Result is ObjectResult)
            {
                var result = (ObjectResult)context.Result;
                if (result.Value is IConcurrencyVersion)
                {
                    context.HttpContext.Response.Headers["ETag"] = ((IConcurrencyVersion)result.Value).RowVersion.ToString();
                }
            }            
        }
    }
}
