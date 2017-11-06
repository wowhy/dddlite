namespace DDDLite.WebApi.Internal
{
  using System;
  using System.Linq;
  using System.Linq.Expressions;
  using DDDLite.CQRS.Messaging;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.DependencyInjection;

  public class DynamicHandlerRegister
  {
    private readonly IApplicationBuilder app;
    private readonly IServiceProvider provider;
    private readonly IHandlerRegister register;
    private readonly Type handlersType;
    private readonly Type handlerType;

    public DynamicHandlerRegister(IApplicationBuilder app, IServiceProvider provider, IHandlerRegister register, Type handlersType, Type handlerType)
    {
      this.handlerType = handlerType;
      this.handlersType = handlersType;
      this.app = app;
      this.provider = provider;
      this.register = register;
    }

    public IApplicationBuilder Register()
    {
      var interfaces = handlersType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);

      foreach (var interfaceType in interfaces)
      {
        var handler = BuildHandler(interfaceType);
        this.InvokeRegisterHandler(handler);
      }

      return app;
    }

    private void InvokeRegisterHandler(object handler)
    {
      var parameterType = handler.GetType().GetGenericArguments()[0];
      var method = typeof(IHandlerRegister).GetMethod("RegisterHandler").MakeGenericMethod(parameterType);
      method.Invoke(register, new object[] { handler });
    }

    private object BuildHandler(Type interfaceType)
    {
      var handleMethod = interfaceType.GetInterfaces()[0].GetMethod("HandleAsync");
      var getServiceMethod = typeof(ServiceProviderServiceExtensions).GetMethod("GetService").MakeGenericMethod(handlersType);
      var parameter = handleMethod.GetParameters().First();

      var parameterExpr = Expression.Parameter(parameter.ParameterType, "message");
      var providerExpr = Expression.Call(null, getServiceMethod, Expression.Constant(provider));
      var bodyExpr = Expression.Call(providerExpr, handleMethod, parameterExpr);

      return Expression.Lambda(bodyExpr, parameterExpr).Compile();
    }
  }
}