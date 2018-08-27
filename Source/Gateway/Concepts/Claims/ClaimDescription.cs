/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Claims
{
    /// <summary>
    /// Represents a description of a claim
    /// </summary>
    public class ClaimDescription : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a claim description to a <see cref="ClaimDescription"/>
        /// </summary>
        /// <param name="claimDescription">Claim as <see cref="string"/> </param>
        public static implicit operator ClaimDescription(string claimDescription) => new ClaimDescription { Value = claimDescription };
    }
}