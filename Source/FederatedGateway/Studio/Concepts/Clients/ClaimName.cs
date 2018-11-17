/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Clients
{
    /// <summary>
    /// Represents a name of a claim
    /// </summary>
    public class ClientName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a claim name to a <see cref="ClientName"/>
        /// </summary>
        /// <param name="clientName">Claim as <see cref="string"/> </param>
        public static implicit operator ClientName(string clientName) => new ClientName { Value = clientName };
    }
}