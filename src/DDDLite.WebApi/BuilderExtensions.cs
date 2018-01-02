namespace DDDLite.WebApi
{
  using System;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Messaging;
  using DDDLite.WebApi.Internal;
  using DDDLite.WebApi.Internal.Versioning;
  using DDDLite.WebApi.Middleware;
  using DDDLite.WebApi.Provider;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.DependencyInjection;
  using DDDLite.CQRS.Events;
  using Microsoft.AspNetCore.Mvc.Infrastructure;
  using Microsoft.Extensions.DependencyInjection.Extensions;

  public static class BuilderExtensions
  {
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
      services.AddMvc();
      services.AddApiVersioning(opt =>
      {
        opt.ReportApiVersions = true;
        opt.ErrorResponses = new JsonErrorResponseProvider();
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.DefaultApiVersion = new ApiVersion(1, 0);
      });



      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddOperatorProvider();

      services.Configure<MvcJsonOptions>(options =>
      {
        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
      });

      return services;
    }

    public static IServiceCollection AddOperatorProvider(this IServiceCollection services)
    {
      return services.AddScoped<IOperatorProvider, DefaultOperatorProvider>();
    }

    public static IApplicationBuilder UseWebApi(this IApplicationBuilder app)
    {
      return app.UseWebApiErrorHandler().UseMvc();
    }

    public static IApplicationBuilder UseWebApiErrorHandler(this IApplicationBuilder app)
    {
      return app.UseMiddleware<WebApiExceptionMiddleware>();
    }

    public static DynamicHandlerRegister BeginRegisterCommandHandlers(this IServiceProvider provider, IHandlerRegister register)
    {
      return new DynamicHandlerRegister(provider, register, typeof(ICommandHandler<>));
    }

    public static DynamicHandlerRegister BeginRegisterEventHandlers(this IServiceProvider provider, IHandlerRegister register)
    {
      return new DynamicHandlerRegister(provider, register, typeof(IEventHandler<>));
    }
  }
}