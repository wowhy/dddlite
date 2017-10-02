namespace Example.IdentityServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using AspNet.Security.OAuth.Introspection;
    using AspNet.Security.OpenIdConnect.Primitives;
    using OpenIddict.Core;
    using OpenIddict.Models;

    using DDDLite.WebApi;
    using DDDLite.WebApi.Data;

    using Example.IdentityServer.Data;

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

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                    options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                })
                .AddEntityFrameworkStores<ExampleIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddCors()
                    .AddWebApi();
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
               .UseWebApi();

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

                if (await manager.FindByClientIdAsync("resource_server", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "resource_server"
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

            services.AddAuthentication()
                    .AddOAuthIntrospection(options =>
                    {
                        options.Authority = new Uri("http://localhost:5000/");
                        options.Audiences.Add("resource_server");
                        options.ClientId = "resource_server";
                        options.ClientSecret = "qwe123,./";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<ExampleIdentityDbContext>()
                       .AddMvcBinders();

                options.EnableAuthorizationEndpoint("/connect/authorize")
                       .EnableTokenEndpoint("/connect/token")
                       .EnableLogoutEndpoint("/connect/logout")
                       .EnableIntrospectionEndpoint("/connect/introspect")
                       .EnableUserinfoEndpoint("/api/v1/userinfo");

                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.DisableHttpsRequirement();

                options.AddDevelopmentSigningCertificate();
            });

            return services;
        }
    }
}
