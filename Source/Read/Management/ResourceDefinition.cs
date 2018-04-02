/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Concepts.Claims;
using Concepts.Resources;

namespace Read.Management
{
    /// <summary>
    /// Represents a definition of a resource
    /// </summary>
    public class ResourceDefinition
    {
        /// <summary>
        /// Gets or sets the <see cref="Name"/> of the <see cref="ResourceDefinition"/>
        /// </summary>
        /// <returns></returns>
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DisplayName"/> of the <see cref="ResourceDefinition"/>
        /// </summary>
        public DisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Description"/> of the <see cref="ResourceDefinition"/>
        /// </summary>
        public Description Description { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="ResourceDefinition"/> is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="ResourceDefinition"/> is emphasized when presented to users
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="ResourceDefinition"/> should be visible in discovery document
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimName">claims</see> that are user specific
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClaimName> UserClaims { get; set; }
    }
}