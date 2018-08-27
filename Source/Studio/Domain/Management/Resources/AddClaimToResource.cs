/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts;
using Concepts.Claims;
using Concepts.Resources;
using Dolittle.Commands;

namespace Domain.Management.Resources
{
    /// <summary>
    /// Represents the intent to add a claim to an identity resource
    /// </summary>
    public class AddClaimToIdentityResource : ICommand
    {
        /// <summary>
        /// Gets or sets the name of the identity resource
        /// </summary>
        public Name Name {  get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Claim"/> that will be added - this is the identifier of the claim
        /// </summary>
        public Claim Claim { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimValue">value</see> for the <see cref="Claim"/> to add
        /// </summary>
        public ClaimValue Value { get; set; }

    }
}