/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Scopes
{
    /// <summary>
    /// Represents a name of a scope
    /// </summary>
    public class ScopeName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a scope name to a <see cref="ScopeName"/>
        /// </summary>
        /// <param name="scopeName">Scope as <see cref="string"/> </param>
        public static implicit operator ScopeName(string scopeName) => new ScopeName { Value = scopeName };
    }
}