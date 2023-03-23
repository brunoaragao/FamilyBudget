using Duende.IdentityServer.Models;

namespace Identity.API;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        Array.Empty<IdentityResource>();

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("budget-api", "Budgeting API")
            };

    public static IEnumerable<Client> Clients(IConfiguration configuration) =>
        new Client[]
            {
                new Client
                {
                    ClientId = "budget-swagger",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "budget-api" },

                    ClientSecrets = { new Secret(configuration["Clients:BudgetSwagger:Secret"].Sha256()) },

                    AllowedCorsOrigins = configuration["Clients:BudgetSwagger:AllowedCorsOrigins"].Split(","),
                }
            };
}