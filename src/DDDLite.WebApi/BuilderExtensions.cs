namespace DDDLite.WebApi
{
    using DDDLite.WebApi.Middleware;
    using Microsoft.AspNetCore.Builder;
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
        }

        public static void UseWebApi(this IApplicationBuilder app)
        {
            app.UseCors("default");
            app.UseMvc();
        }
    }
}