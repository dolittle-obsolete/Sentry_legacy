/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Web
{
    /// <summary>
    /// Represents a client configuration in an <see cref="Application"/>
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Gets or sets the CliendId that is used to identity the client
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the allowed grant types
        /// </summary>
        public IEnumerable<string> AllowedGrantTypes { get; set; }

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
        public IEnumerable<string> AllowedScopes { get; set; }
    }
}