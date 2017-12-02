namespace DDDLite.WebApi
{
  using System;
  using System.Linq;
  using System.Linq.Expressions;
  using DDDLite.CQRS.Messaging;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.DependencyInjection;

  public class DynamicHandlerRegister
  {
    private IServiceProvider provider => this.httpContextAccessor.HttpContext.RequestServices;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IHandlerRegister register;
    private readonly Type handlerType;

    public DynamicHandlerRegister(IHttpContextAccessor httpContextAccessor, IHandlerRegister register, Type handlerType)
    {
      this.httpContextAccessor = httpContextAccessor;
      this.register = register;
      this.handlerType = handlerType;
    }

    public DynamicHandlerRegister Register<THandlers>()
    {
      var handlersType = typeof(THandlers);
      var interfaces = handlersType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);

      foreach (var interfaceType in interfaces)
      {
        var handler = BuildHandler(interfaceType, handlersType);
        this.InvokeRegisterHandler(handler);
      }

      return this;
    }

    private void InvokeRegisterHandler(object handler)
    {
      var parameterType = handler.GetType().GetGenericArguments()[0];
      var method = typeof(IHandlerRegister).GetMethod("RegisterHandler").MakeGenericMethod(parameterType);
      method.Invoke(register, new object[] { handler });
    }

    private object BuildHandler(Type interfaceType, Type handlersType)
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