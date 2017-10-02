namespace Example.IdentityServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Example.IdentityServer.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;
    using DDDLite.WebApi.Data;
    using Microsoft.AspNetCore.Identity;
    using AspNet.Security.OpenIdConnect.Primitives;
    using Microsoft.AspNetCore.Mvc;
    using OpenIddict.Core;
    using OpenIddict.Models;
    using System.Threading;
    using AspNet.Security.OAuth.Validation;

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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ExampleIdentityDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
                options.UseOpenIddict();
            });

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

            // services.AddIdentity<ApplicationUser, ApplicationRole>(options => 
            //     {
            //         options.Password.RequireDigit = false;
            //         options.Password.RequiredLength = 6;
            //         options.Password.RequireNonAlphanumeric = false;
            //         options.Password.RequireUppercase = false;
            //         options.Password.RequireLowercase = false;

            //         options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
            //         options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
            //         options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            //     })
            //     .AddEntityFrameworkStores<ExampleIdentityDbContext>();

            services.AddIdentityServer()
                    .AddCors()
                    .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseAuthentication()
               .UseMvc();

            await app.InitializeDatabase();
        }
    }

    public static class StartupExtensions
    {
        public static async Task InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ExampleIdentityDbContext>();
                context.Database.EnsureCreated();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();
                var cancellationToken = default(CancellationToken);

                if (await manager.FindByClientIdAsync("webapp", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "webapp",
                        DisplayName = "WebApp",
                        LogoutRedirectUri = "http://localhost:9000/logout",
                        RedirectUri = "http://localhost:9000/login"
                    };

                    await manager.CreateAsync(application, cancellationToken);
                }

                if (await manager.FindByClientIdAsync("webapi", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "webapi"
                    };

                    await manager.CreateAsync(application, "qwe123,./", cancellationToken);
                }
            }
        }

        public static IServiceCollection AddIdentityServer(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
                    .AddOAuthValidation();

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<ExampleIdentityDbContext>()
                       .AddMvcBinders();

                options.EnableAuthorizationEndpoint("/connect/authorize")
                       .EnableTokenEndpoint("/connect/token")
                       .EnableLogoutEndpoint("/connect/logout")
                       .EnableIntrospectionEndpoint("/connect/introspect")
                       .EnableUserinfoEndpoint("/api/v1/userinfo");

                options.AllowImplicitFlow()
                       .AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.DisableHttpsRequirement();

                options.AddDevelopmentSigningCertificate();

                options.UseJsonWebTokens();
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            return services;
        }
    }
}
