namespace DDDLite.WebApi.Internal
{
  using System;
  using System.Security.Claims;
  using DDDLite.WebApi.Provider;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.Options;

  internal class CurrentUserProvider : ICurrentUserProvider
  {
    private IHttpContextAccessor accessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
      this.accessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
      var user = accessor.HttpContext.User;
      if (user.Identity.IsAuthenticated)
      {
        var claim = user.FindFirst(k => k.Type == "sub");
        if (string.IsNullOrEmpty(claim.Value))
        {
          return null;
        }

        return claim.Value;
      }

      return null;
    }
  }
}