/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    /// <summary>
    /// Represents an implementation of <see cref="IClientStore"/>
    /// </summary>
    public class ClientStore : IClientStore
    {
        readonly IAuthContext _authContext;
        private readonly ILogger<ClientStore> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authContext"></param>
        /// <param name="logger"></param>
        public ClientStore(IAuthContext authContext,ILogger<ClientStore> logger)
        {
            _authContext = authContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task<Client> FindClientByIdAsync(string clientIdAsString)
        {
            Guid clientId;
            if( !Guid.TryParse(clientIdAsString, out clientId) ) 
            {
                throw new ArgumentException("ClientId is not a valid Guid");
                // Todo: proper exception
            }

            var client = _authContext.Application.Clients.FirstOrDefault(c => c.Id == clientId);
            if( client == null )
            {
                throw new ArgumentException("Unknown client");
                // Todo: proper exception
            }

            var idsrvClient = new Client
            {
                ClientId = clientIdAsString,
                AllowedGrantTypes = client.AllowedGrantTypes.ToList(),
                RedirectUris = client.RedirectUris.ToList(),
                PostLogoutRedirectUris = client.PostLogooutRedirectUris.ToList(),
                AllowedScopes = client.AllowedScopes.ToList(),
                AllowedCorsOrigins = client.RedirectUris.Select(url => {
                    var uri = new Uri(url);
                    var origin = uri.AbsoluteUri.Substring(0,uri.AbsoluteUri.Length-uri.AbsolutePath.Length);
                    if( origin.EndsWith("/")) origin = origin.Substring(origin.Length-1);
                    return origin;
                }).ToList()
            };

            
            _logger.LogInformation($"Client found - allowed CORS : [{string.Join(", ",idsrvClient.AllowedCorsOrigins)}]");

            return Task.FromResult(idsrvClient);
        }
    }
}