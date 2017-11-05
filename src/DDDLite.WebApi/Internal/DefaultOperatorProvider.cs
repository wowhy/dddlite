namespace DDDLite.WebApi.Internal
{
  using System;
  using System.Security.Claims;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.Options;

  using DDDLite.Domain;
  using DDDLite.WebApi.Provider;

  internal class DefaultOperatorProvider : IOperatorProvider
  {
    private IHttpContextAccessor accessor;

    public DefaultOperatorProvider(IHttpContextAccessor httpContextAccessor)
    {
      this.accessor = httpContextAccessor;
    }

    public Operator GetCurrentOperator()
    {
      var user = accessor.HttpContext.User;
      if (user.Identity.IsAuthenticated)
      {
        var userIdClaim = user.FindFirst("sub");
        if (string.IsNullOrEmpty(userIdClaim.Value))
        {
          return null;
        }

        return new Operator
        {
          UserId = userIdClaim.Value,
          UserName = user.FindFirst("name").Value
        };
      }

      return null;
    }
  }
}