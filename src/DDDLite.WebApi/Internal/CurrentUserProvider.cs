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
        private IOptions<IdentityOptions> optionsAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> optionsAccessor)
        {
            this.accessor = httpContextAccessor;
            this.optionsAccessor = optionsAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            var context = accessor.HttpContext;
            if (context.User.Identity.IsAuthenticated)
            {
                var type = optionsAccessor.Value.ClaimsIdentity.UserIdClaimType;
                var claim = context.User.FindFirst(k => k.Type == type);
                return Guid.Parse(claim.Value);
            }

            return null;
        }
    }
}