namespace Sample.WebApi
{
    using System;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Cors;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using DDDLite.CommandStack.Repository;
    using DDDLite.WebApi;

    using Core.Domain;
    using Core.CommandStack.Repository;
    using Core.QueryStack.Repository;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                config =>
                {
                    config.Filters.Add(new ApiExceptionFilter());
                    config.Filters.Add(new ApiCorsFilter());
                }
            );

            // register command repository context
            services.AddDbContext<SampleDomainDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=hongyuan;Database=sample;"));
            services.AddRepositoryContext<ISampleDomainRepositoryContext, SampleDomainRepositoryContext>();
            services.AddRepository<ISampleDomainRepositoryContext, IDomainRepository<Blog>>(context => context.GetRepository<Blog>());

            // register query repository context
            services.AddDbContext<SampleReadonlyDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=hongyuan;Database=sample;"));
            services.AddRepositoryContext<ISampleQueryRepositoryContext, SampleQueryRepositoryContext>();

            services.AddAssembly(Assembly.Load(new AssemblyName("Sample.Core")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider service)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseCors("*");

            service.GetService<SampleDomainDbContext>().Database.EnsureCreated();
        }
    }
}