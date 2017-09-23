namespace Example.IdentityWebApi.Configuration
{
    using System.Collections.Generic;

    using IdentityServer4;
    using IdentityServer4.Models;

    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    ClientSecrets =
                    {
                        new Secret("qwe123,./".Sha256())
                    },
                    AllowRememberConsent = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = 
                    {
                        "api"
                    }
                }
            };
        }
    }
}