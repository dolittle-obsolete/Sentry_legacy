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
    public class ScopeDescription : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> representation of a scope description to a <see cref="ScopeDescription"/>
        /// </summary>
        /// <param name="scopeDescription">Scope as <see cref="string"/> </param>
        public static implicit operator ScopeDescription(string scopeDescription) => new ScopeDescription { Value = scopeDescription };
    }
}