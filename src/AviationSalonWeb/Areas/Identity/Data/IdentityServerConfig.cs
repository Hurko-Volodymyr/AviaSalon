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
            ClientId = "aviation_salon_client",
            ClientName = "Aviation Salon Web Client",
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:7267/home/index" },
            PostLogoutRedirectUris = { "https://localhost:7267/home/index" },
            ClientSecrets = { new Secret("aviation_salon_secret".Sha256()) },
            RequireConsent = false,
            RequirePkce = true,
            AllowedScopes = { "openid", "profile", "api1" }
        },
    };

    }


}
