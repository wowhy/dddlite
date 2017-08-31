namespace DDDLite.WebApi.Internal
{
    using System;
    using System.Security.Claims;
    using DDDLite.WebApi.Provider;
    using Microsoft.AspNetCore.Http;

    internal class CurrentUserProvider : ICurrentUserProvider
    {
        private IHttpContextAccessor accessor;
        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.accessor = httpContextAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            if (accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var claim = accessor.HttpContext.User.FindFirst(k => k.Type == ClaimTypes.NameIdentifier);
                return Guid.Parse(claim.Value);
            }

            return null;
        }
    }
}