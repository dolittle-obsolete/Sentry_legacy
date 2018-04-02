/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.DependencyInversion;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientStore : IClientStore
    {
        readonly IContainer _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public ClientStore(IContainer container)
        {
            _container = container;
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

            var authContext = _container.Get<AuthContext>();
            var client = authContext.Application.Clients.FirstOrDefault(c => c.Id == clientId);
            if( client == null )
            {
                throw new ArgumentException("Unknown client");
                // Todo: proper exception
            }

            return Task.FromResult(new Client
            {
                ClientId = clientIdAsString,
                AllowedGrantTypes = client.AllowedGrantTypes.ToList(),
                RedirectUris = client.RedirectUris.ToList(),
                PostLogoutRedirectUris = client.PostLogooutRedirectUris.ToList(),
                AllowedScopes = client.AllowedScopes.ToList()
            });
        }
    }
}