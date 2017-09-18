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
                        new Secret("qwe123m,.".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}