namespace DDDLite.WebApi
{
    using DDDLite.WebApi.Internal;
    using DDDLite.WebApi.Middleware;
    using DDDLite.WebApi.Provider;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class BuilderExtensions
    {
        public static void AddWebApi(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("default", policy =>
                {
                    policy.AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(WebApiExceptionFilter));
            });
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
            });
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        }

        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void UseWebApi(this IApplicationBuilder app)
        {
            app.UseCors("default");
            app.UseMvc();
        }
    }
}