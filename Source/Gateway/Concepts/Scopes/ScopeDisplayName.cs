/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Scopes
{
    /// <summary>
    /// Represents a description of a scope
    /// </summary>
    public class ScopeDisplayName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a scope display name to a <see cref="ScopeDisplayName"/>
        /// </summary>
        /// <param name="scopeDisplayName">Scope as <see cref="string"/> </param>
        public static implicit operator ScopeDisplayName(string scopeDisplayName) => new ScopeDisplayName { Value = scopeDisplayName };
    }
}