using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace DDDLite.WebApi.Mvc.Auth
{
    public class SimpleTokenAuthenticationMiddleware : AuthenticationMiddleware<SimpleTokenAuthenticationOptions>
    {
        public SimpleTokenAuthenticationMiddleware(
            RequestDelegate next,
            IOptions<SimpleTokenAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<SimpleTokenAuthenticationOptions> CreateHandler()
        {
            return new SimpleTokenAuthenticationHandler();
        }
    }
}
