namespace Example.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

    using DDDLite.WebApi;
    using DDDLite.Repositories;
    using DDDLite.Repositories.EntityFramework;
    using DDDLite.WebApi.Data;

    using Example.Core.Domain;
    using Example.Repositories.EntityFramework;
    using AspNet.Security.OAuth.Introspection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("Default");

            services.AddWebApi();
            services.AddAuthentication(OAuthIntrospectionDefaults.AuthenticationScheme)
                .AddOAuthIntrospection(options =>
                {
                    options.Authority = new Uri("http://localhost:5000/");
                    options.Audiences.Add("resource_server");
                    options.ClientId = "resource_server";
                    options.ClientSecret = "qwe123,./";
                    options.RequireHttpsMetadata = false;
                });

            services.AddDbContext<ExampleDbContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddScoped<IRepository<Order>, EFRepositoryBase<Order>>();
            services.AddScoped<IRepository<Product>, EFRepositoryBase<Product>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseAuthentication()
               .UseWebApi()
               .InitializeDatabase();
        }
    }

    public static class StartupExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ExampleDbContext>().Database.Migrate();
            }

            return app;
        }
    }
}
