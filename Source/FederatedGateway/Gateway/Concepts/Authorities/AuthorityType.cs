/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Concepts.Authorities
{
    /// <summary>
    /// Represents the concept of a unique identifier for a security authority
    /// </summary>
    public class AuthorityType : ConceptAs<Guid>
    {
        /// <summary>
        /// Implicitally convert from <see cref="Guid"/> to <see cref="AuthorityType"/>
        /// </summary>
        /// <param name="authorityType"><see cref="Guid">Unique identifier</see> representing the authority type</param>
        public static implicit operator AuthorityType(Guid authorityType) => new AuthorityType { Value = authorityType };
    }
}