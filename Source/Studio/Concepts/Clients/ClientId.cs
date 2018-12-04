/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Concepts.Clients
{
    /// <summary>
    /// Represents the concept of a unique identifier for a security authority
    /// </summary>
    public class ClientId : ConceptAs<Guid>
    {
        /// <summary>
        /// Implicitally convert from <see cref="Guid"/> to <see cref="ClientId"/>
        /// </summary>
        /// <param name="id"><see cref="Guid">Unique identifier</see> representing the authority</param>
        public static implicit operator ClientId(Guid id) => new ClientId { Value = id };
    }
}