﻿namespace Example.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

    using DDDLite.WebApi;
    using DDDLite.Repositories;
    using DDDLite.Repositories.EntityFramework;

    using Example.Core.Domain;
    using Example.Repositories.EntityFramework;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi();
            services.AddDbContext<ExampleDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("Default")));
            services.AddScoped<IRepository<Order>, EFRepository<Order>>(provider => new EFRepository<Order>(provider.GetService<ExampleDbContext>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ExampleDbContext dbContext)
        {
            app.UseWebApi();
            dbContext.Database.EnsureCreated();
        }
    }
}
