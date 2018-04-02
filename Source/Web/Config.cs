// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class NationalSociety : IdentityResource
    {
        /// <summary>
        /// 
        /// </summary>
        public NationalSociety()
        {
            Name = "nationalsociety";
            DisplayName = "National Society you belong to";
            Required = true;
            UserClaims.Add("nationalsociety");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new NationalSociety()
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "25c7ddac-dd1b-482a-8638-aaa909fd1f1c",
                    ClientName = "CBS",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = { 
                        "http://localhost:5002/signin-oidc", 
                        "http://localhost:5000/signin-oidc",
                        "http://localhost:5000/Registration/RequestAccessOidcCallback"
                    },
                    PostLogoutRedirectUris = { 
                        "http://localhost:5002/signout-callback-oidc", 
                        "http://localhost:5000/signout-callback-oidc" 
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        "nationalsociety"
                    }//,

                    //AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}