/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Claims
{
    /// <summary>
    /// Rerepsents a value of a claim
    /// </summary>
    public class ClaimValue : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a claim name to a <see cref="ClaimValue"/>
        /// </summary>
        /// <param name="claimValue">Claim value as <see cref="string"/> </param>
        public static implicit operator ClaimValue(string claimValue) => new ClaimValue { Value = claimValue };
    }
}