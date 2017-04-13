namespace DDDLite.WebApi.Mvc.Auth
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SimpleTokenAuthenticationHandler : AuthenticationHandler<SimpleTokenAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Options.TokenValidatorFactory().CanReadToken(Request))
            {
                var principal = Options.TokenValidatorFactory().ValidateToken(Request);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);
                return Task.FromResult(AuthenticateResult.Success(ticket));
                
            }
            return Task.FromResult(AuthenticateResult.Skip());
        }
    }
}
