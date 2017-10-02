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
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using AspNet.Security.OAuth.Validation;
    using AspNet.Security.OpenIdConnect.Primitives;

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
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("Default");

            services.AddWebApi();

            services.AddDbContext<ExampleDbContext>(opt => opt.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
            services.AddDbContext<ExampleIdentityDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
                opt.UseOpenIddict();
            });

            services.AddScoped<IRepository<Order>, EFRepositoryBase<Order>>();
            services.AddScoped<IRepository<Product>, EFRepositoryBase<Product>>();

            var builder = services.AddIdentityCore<ApplicationUser>(opt =>
                                    {
                                        opt.Password.RequireDigit = false;
                                        opt.Password.RequiredLength = 6;
                                        opt.Password.RequireNonAlphanumeric = false;
                                        opt.Password.RequireUppercase = false;
                                        opt.Password.RequireLowercase = false;

                                        opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                                        opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                                        opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                                    });
            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);
            builder
                .AddEntityFrameworkStores<ExampleIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleValidator<RoleValidator<ApplicationRole>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddOpenIddict(opt =>
            {
                opt.AddEntityFrameworkCoreStores<ExampleIdentityDbContext>();
                opt.AddMvcBinders();
                opt.EnableAuthorizationEndpoint("/connect/authorize");
                opt.EnableTokenEndpoint("/connect/token");
                opt.AllowPasswordFlow();
                opt.AllowRefreshTokenFlow();
                opt.DisableHttpsRequirement();
            });

            services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
                .AddOAuthValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseAuthentication();
            app.UseWebApi();
            app.InitializeDatabase();
        }
    }

    public static class StartupExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ExampleDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ExampleIdentityDbContext>().Database.Migrate();
            }

            return app;
        }
    }
}
