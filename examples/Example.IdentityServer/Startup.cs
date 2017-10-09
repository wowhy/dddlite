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
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

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
                })
                .AddEntityFrameworkStores<ExampleIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddJWTAuthentication()
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

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<ExampleIdentityDbContext>()
                       .AddMvcBinders();

                options.EnableTokenEndpoint("/api/v1/connect/token")
                       .EnableIntrospectionEndpoint("/api/v1/connect/introspect")
                       .EnableUserinfoEndpoint("/api/v1/userinfo");

                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.DisableHttpsRequirement();

                options.AddDevelopmentSigningCertificate();

                options.UseJsonWebTokens();
            });

            return services;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services) 
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            
            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.Authority = "http://localhost:5000/";
                        options.Audience = "resource_server";
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = OpenIdConnectConstants.Claims.Subject,
                            RoleClaimType = OpenIdConnectConstants.Claims.Role,
                            ValidateIssuer = false
                        };
                    });

            return services;
        }
    }
}
