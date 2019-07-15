/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Concepts.Grants;
using Concepts.Clients;
using Dolittle.ReadModels;
using Concepts.Scopes;

namespace Read.Clients
{
    /// <summary>
    /// Represents a client configuration
    /// </summary>
    public class Client : IReadModel
    {
        /// <summary>
        /// Gets or sets the <see cref="ClientId"/> that is used to identity the client
        /// </summary>
        public ClientId Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the client
        /// </summary>
        public ClientName Name { get; set; }

        /// <summary>
        /// Gets or sets the allowed grant types
        /// </summary>
        public IEnumerable<GrantType> AllowedGrantTypes { get; set; }

        /// <summary>
        /// Gets or sets the allowed redirect Uris
        /// </summary>
        public IEnumerable<string> RedirectUris { get; set; }

        /// <summary>
        /// Gets or sets the allowed post logout redirect Uris
        /// </summary>
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }

        /// <summary>
        /// Gets or sets the scopes that are allowed
        /// </summary>
        public IEnumerable<ScopeName> AllowedScopes { get; set; }

        /// <summary>
        /// Gets or sets whether or not to allow offline access. Defaults to false.
        /// </summary>
        public bool AllowOfflineAccess { get; set; }

        /// <summary>
        /// Gets or sets whether or not to require client secret for requesting tokens at the token endpoint
        /// </summary>
        public bool RequireClientSecret { get; set; }
    }
}