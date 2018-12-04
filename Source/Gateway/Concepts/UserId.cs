/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Concepts
{
    /// <summary>
    /// Represents the concept that identifies a user
    /// </summary>
    public class UserId : ConceptAs<Guid>
    {
        /// <summary>
        /// Implicitly convert from <see cref="Guid"/> to <see cref="UserId"/>
        /// </summary>
        /// <param name="userId"><see cref="Guid"/> to convert from</param>
        public static implicit operator UserId(Guid userId) => new UserId { Value = userId };
    }
}