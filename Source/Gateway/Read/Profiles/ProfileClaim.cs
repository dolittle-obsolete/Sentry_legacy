/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Concepts.Claims;
using Concepts.Resources;
using Dolittle.ReadModels;
using Read.Resources;

namespace Read.Profiles
{
    /// <summary>
    /// Represents additional claims for a profile - typically collected during registration
    /// </summary>
    public class ProfileClaim : IReadModel
    {
        /// <summary>
        /// Gets or sets the name of <see cref="Claim"/>
        /// </summary>
        public ClaimName Name { get; set; }

        /// <summary>
        /// Gets or sets the displayed name of <see cref="Claim"/>
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimDescription">description</see> of a <see cref="Claim"/>
        /// </summary>
        public ClaimDescription Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="Resource"/>
        /// </summary>
        public ClaimType Type { get; set; }

        /// <summary>
        /// Gets or sets the allowed <see cref="ClaimValue">claim values</see>
        /// </summary>
        /// <remarks>
        /// This is typically used by <see cref="ClaimType.Select"/> types
        /// </remarks>
        public IEnumerable<ClaimValue> AllowedValues { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="Claim"/> is required
        /// </summary>
        public bool Required { get; set; }
    }
}