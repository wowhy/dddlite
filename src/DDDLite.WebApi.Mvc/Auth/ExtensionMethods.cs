namespace DDDLite.WebApi.Mvc.Auth
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.DependencyInjection;

    public static class ExtensionMethods
    {
        public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return UseSimpleTokenAuthentication(app, new SimpleTokenAuthenticationOptions());
        }

        public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app, SimpleTokenAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.AuthenticationScheme))
            {
                options.AuthenticationScheme = "Bearer";
            }

            if (options.TokenValidatorFactory == null)
            {
                options.TokenValidatorFactory = () => app.ApplicationServices.GetService<ITokenValidator>();
            }

            return app.UseMiddleware<SimpleTokenAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
