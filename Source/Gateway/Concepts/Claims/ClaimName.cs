/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Claims
{
    /// <summary>
    /// Represents a name of a claim
    /// </summary>
    public class ClaimName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a claim name to a <see cref="ClaimName"/>
        /// </summary>
        /// <param name="claimName">Claim as <see cref="string"/> </param>
        public static implicit operator ClaimName(string claimName) => new ClaimName { Value = claimName };
    }
}