namespace Sample.WebApi
{
    using System;
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

    using Core.Domain;
    using Core.Repository;

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

            // register command sender
            services.AddSingleton<InProcessCommandBus>();
            services.AddSingleton<ICommandSender>((provider) => provider.GetService<InProcessCommandBus>());
            services.AddSingleton<IMessageSubscriber>((provider) => provider.GetService<InProcessCommandBus>());
            services.AddSingleton<ICommandService>((provider) => new CommandService(provider.GetService<IMessageSubscriber>(), provider));

            // register command repository context
            services.AddDbContext<SampleDomainDbContext>(options => options.UseNpgsql(this.Configuration.GetConnectionString("WriteConnection")));
            services.AddRepositoryContext<ISampleDomainRepositoryContext, SampleDomainRepositoryContext>();
            services.AddRepository<ISampleDomainRepositoryContext, IDomainRepository<Blog>>(context => context.GetRepository<Blog>());

            // register query repository context
            services.AddDbContext<SampleReadonlyDbContext>(options => options.UseNpgsql(this.Configuration.GetConnectionString("ReadConnection")));
            services.AddRepositoryContext<ISampleQueryRepositoryContext, SampleQueryRepositoryContext>();

            var assembly = Assembly.Load(new AssemblyName("Sample.Core"));
            var register = new Register(services);
            register.RegisterCommands(assembly);
            register.RegisterValidators(assembly);
            register.RegisterCommandHandlers(assembly);
            register.RegisterQueryServices(assembly);
            register.RegisterAutoMapper(assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider service)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseCors("*");

            service.GetService<SampleDomainDbContext>().Database.EnsureCreated();
            service.GetService<ICommandService>().Subscriber.Subscribe();
        }
    }
}