namespace DDDLite.WebApi
{
    using System;
    using System.Security.Claims;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return Guid.Parse(user.FindFirst(ClaimTypes.Sid).Value);
            }
            else
            {
                throw new AuthorizedException(401, "�����֤ʧ�ܣ�");
            }
        }
    }
}