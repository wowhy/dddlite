namespace DDDLite.WebApi.Internal.Versioning
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using static System.String;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Versioning;

    internal class JsonErrorResponseProvider : DefaultErrorResponseProvider
    {
        protected override IDictionary<string, object> CreateErrorContent(ErrorResponseContext context)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            var comparer = StringComparer.OrdinalIgnoreCase;
            var error = new Dictionary<string, object>(comparer);
            var root = new Dictionary<string, object>(comparer) { ["error"] = error };

            if (!IsNullOrEmpty(context.ErrorCode))
            {
                error["code"] = context.ErrorCode;
            }

            if (!IsNullOrEmpty(context.Message))
            {
                error["message"] = context.Message;
            }

            if (!IsNullOrEmpty(context.MessageDetail))
            {
                var environment = (IHostingEnvironment)context.Request.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment));

                if (environment?.IsDevelopment() == true)
                {
                    error["innerError"] = new Dictionary<string, object>(comparer) { ["message"] = context.MessageDetail };
                }
            }

            return root;
        }
    }
}