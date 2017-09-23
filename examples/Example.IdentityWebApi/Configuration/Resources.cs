namespace Example.IdentityWebApi.Configuration
{
    using System.Collections.Generic;

    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Models;

    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                // custom identity resource with some consolidated claims
                new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                // simple version with ctor
                new ApiResource("api", "WEB API")
                {
                    // this is needed for introspection when using reference tokens
                    ApiSecrets = { new Secret("qwe123,./t".Sha256()) }
                }
            };
        }
    }
}