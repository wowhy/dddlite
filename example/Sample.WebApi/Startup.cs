namespace Sample.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using DDDLite.Commands;
    using DDDLite.Messaging;
    using DDDLite.Repository;
    using DDDLite.WebApi;
    using DDDLite.Config;

    using Core.Entity;
    using Core.Repository;
    using Core.Querying;
    using DDDLite.Events;
    using Core.Commands;
    using Core.Config;

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
            services.AddDbContext<SampleMasterDbContext>(options => options.UseNpgsql(this.Configuration.GetConnectionString("WriteConnection")));
            services.AddScoped<IDomainRepository<Blog>, SampleDomainRepository<Blog>>();

            // register query repository context
            services.AddDbContext<SampleReadonlyDbContext>(options => options.UseNpgsql(this.Configuration.GetConnectionString("ReadConnection")));
            services.AddScoped<IQueryRepository<Blog>, SampleQueryRepository<Blog>>();

            var assembly = Assembly.Load(new AssemblyName("Sample.Core"));
            var register = new SampleRegister(services);
            register.Load(assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider service)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseCors("*");

            service.GetService<SampleMasterDbContext>().Database.EnsureCreated();
            service.GetService<ICommandConsumer>().Subscriber.Subscribe();
            service.GetService<IEventConsumer>().Subscriber.Subscribe();
        }
    }
}