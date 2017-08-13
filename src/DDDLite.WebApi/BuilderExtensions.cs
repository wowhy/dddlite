namespace DDDLite.WebApi
{
    using DDDLite.WebApi.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class BuilderExtensions
    {
        public static void AddWebApi(this IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(WebApiExceptionFilter));
            });
        }

        public static void UseWebApi(this IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}