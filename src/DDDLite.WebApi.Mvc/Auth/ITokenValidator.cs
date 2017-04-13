namespace DDDLite.WebApi.Mvc.Auth
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    public interface ITokenValidator
    {
        bool CanReadToken(HttpRequest requrest);

        ClaimsPrincipal ValidateToken(HttpRequest request);
    }
}
