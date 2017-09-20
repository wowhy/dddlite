namespace DDDLite.WebApi
{
    using DDDLite.WebApi.Exception;
    using DDDLite.WebApi.Internal;
    using DDDLite.WebApi.Internal.Versioning;
    using DDDLite.WebApi.Middleware;
    using DDDLite.WebApi.Provider;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public static class BuilderExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.ErrorResponses = new JsonErrorResponseProvider();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddHttpContextAccessor()
                    .AddCurrentUserProvider();

            return services;
        }

        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
        {
            return services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IServiceCollection AddCurrentUserProvider(this IServiceCollection services)
        {
            return services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app)
        {
            return app.UseWebApiErrorHandler().UseMvc();
        }

        public static IApplicationBuilder UseWebApiErrorHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WebApiExceptionMiddleware>();
        }
    }
}