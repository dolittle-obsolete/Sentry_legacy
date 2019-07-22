using IdentityServer4.Models;
using System.Collections.Generic;

namespace Core
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("dolittle", new [] { "tid" })
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Demo API", new[] { "tid" })
                {
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // device flow
                new Client
                {
                    ClientId = "fda65383-e32a-4694-9c6f-baffbe7bd9af",
                    ClientName = "Device Flow Client",

                    AllowedGrantTypes = GrantTypes.DeviceFlow,
                    RequireClientSecret = false,

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "api", "dolittle" }
                }
            };
        }
    }
}
