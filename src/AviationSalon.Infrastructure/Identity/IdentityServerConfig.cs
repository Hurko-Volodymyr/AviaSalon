using IdentityServer4.Models;

namespace AviationSalon.Infrastructure.Identity
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
            new ApiResource("api1", "My API"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {           
                new Client
                {
                ClientId = "client1",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api1" },
                },
            };
    }


}
