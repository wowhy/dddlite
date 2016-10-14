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

    using DDDLite.Commands;
    using DDDLite.Messaging;
    using DDDLite.Repository;
    using DDDLite.WebApi;

    using Core.Domain;
    using Core.Repository;

    public class Startup
    {
        private ICommandService commandService;
        private InProcessCommandBus commandBus;

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
            this.commandBus = new InProcessCommandBus();
            services.AddSingleton<ICommandSender>((provider) => this.commandBus);

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

            this.commandService = new CommandService(this.commandBus, service);
            this.commandBus.Subscribe();
        }
    }
}