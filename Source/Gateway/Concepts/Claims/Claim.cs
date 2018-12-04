/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Claims
{
    /// <summary>
    /// Represents a claim that is associated with a user
    /// </summary>
    public class Claim : Value<Claim>
    {
        /// <summary>
        /// Gets or sets the <see cref="ClaimName"/>
        /// </summary>
        public ClaimName Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimValues"/>
        /// </summary>
        public ClaimValues Values { get; set; }
    }
}