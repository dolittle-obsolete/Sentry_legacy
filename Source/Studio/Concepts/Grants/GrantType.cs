/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Concepts.Grants
{
    /// <summary>
    /// Represents the concept of a unique identifier for a security authority
    /// </summary>
    public class GrantType : ConceptAs<string>
    {
        /// <summary>
        /// Implicitally convert from <see cref="string"/> to <see cref="GrantType"/>
        /// </summary>
        /// <param name="type"><see cref="string">Unique identifier</see> representing the authority</param>
        public static implicit operator GrantType(string type) => new GrantType { Value = type };
    }
}