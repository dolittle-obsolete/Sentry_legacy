/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Concepts.Claims;
using Concepts.Resources;
using Dolittle.ReadModels;

namespace Infrastructure.Resources
{
    /// <summary>
    /// Represents a definition of a resource
    /// </summary>
    public class Resource : IReadModel
    {
        /// <summary>
        /// Gets or sets the <see cref="Name"/> of the <see cref="Resource"/>
        /// </summary>
        /// <returns></returns>
        public ResourceName Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DisplayName"/> of the <see cref="Resource"/>
        /// </summary>
        public ResourceDisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Description"/> of the <see cref="Resource"/>
        /// </summary>
        public ResourceDescription Description { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="Resource"/> is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="Resource"/> is emphasized when presented to users
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="Resource"/> should be visible in discovery document
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimName">claims</see> that the scope includes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClaimName> UserClaims { get; set; }
    }
}