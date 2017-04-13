using System;
using Akka.Actor;
using Akka.DI.AutoFac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDDLite.Querying;
using DDDLite.Repositories;
using DDDLite.WebApi.Mvc;
using Example.Actors;
using Example.EF.Querying;
using Example.EF.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Example.WebApi
{
    public class Startup
    {
        public static ExampleSystem System { get; set; }

        public IConfigurationRoot Configuration { get; private set; }
        public IContainer Container { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(new ApiExceptionFilter());
                config.Filters.Add(new ConcurrencyVersionFilter());
            });

            var builder = new ContainerBuilder();

            builder.RegisterType<WriteDbContext>()
                   .AsSelf();

            builder.RegisterType<ReadDbContext>()
                   .AsSelf();

            builder.RegisterGeneric(typeof(ExampleQueryService<>))
                   .AsSelf()
                   .As(typeof(IQueryService<>));

            builder.RegisterGeneric(typeof(ExampleDomainRepository<>))
                   .AsSelf()
                   .As(typeof(IDomainRepository<>));

            ExampleSystem.RegisterActorTypes(builder);

            builder.Populate(services);

            this.Container = builder.Build();
            return new AutofacServiceProvider(this.Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseMvc();

            this.EnsureDatabase();
            this.InitMapper();

            var actorSystem = ActorSystem.Create("Example");
            var resolver = new AutoFacDependencyResolver(this.Container, actorSystem);

            System = ExampleSystem.Create(actorSystem, resolver, Container);
            System.Initialize();

            appLifetime.ApplicationStopped.Register(() =>
            {
                System.Shotdown();
                Container.Dispose();
            });
        }

        private void InitMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles("Example.Core");
            });
        }

        private void EnsureDatabase()
        {
            new WriteDbContext().Database.EnsureCreated();
        }
    }
}
