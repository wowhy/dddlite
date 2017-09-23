namespace Example.IdentityWebApi
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
    using Example.IdentityWebApi.Data;
    using Example.IdentityWebApi.Configuration;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using IdentityServer4.AccessTokenValidation;

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
            services.AddDbContext<ExampleIdentityDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("Default")));

            services.AddScoped<IRepository<Order>, EFRepositoryBase<Order>>();
            services.AddScoped<IRepository<Product>, EFRepositoryBase<Product>>();

            var builder = services.AddIdentityCore<ApplicationUser>(opt =>
                                    {
                                        opt.Password.RequireDigit = true;
                                        opt.Password.RequiredLength = 8;
                                        opt.Password.RequireNonAlphanumeric = false;
                                        opt.Password.RequireUppercase = true;
                                        opt.Password.RequireLowercase = true;
                                    });
            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);
            builder
                .AddEntityFrameworkStores<ExampleIdentityDbContext>()
                .AddIdentityServer()
                .AddDefaultTokenProviders()
                .AddRoleValidator<RoleValidator<ApplicationRole>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddIdentityServer()
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.JwtAuthenticationScheme)
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.ApiName = "api";
                    opt.Authority = "http://localhost:5000";
                    opt.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseWebApi();
        }
    }
}
