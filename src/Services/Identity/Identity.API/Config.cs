using Duende.IdentityServer.Models;

namespace Identity.API;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("budget", "Budgeting API")
            };

    public static IEnumerable<Client> Clients(IConfiguration configuration) =>
        new Client[]
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = { new Secret(configuration["ClientSecrets:Client"].Sha256()) },
                    AllowedScopes = { "budget" }
                }
            };
}