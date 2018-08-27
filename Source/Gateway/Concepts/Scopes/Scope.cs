/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Scopes
{
    /// <summary>
    /// Represents the concept of a scope
    /// </summary>
    public class Scope : Value<Scope>
    {
        /// <summary>
        /// Gets or sets the name of the <see cref="Scope"/>
        /// </summary>
        public ScopeName Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the <see cref="Scope"/>
        /// </summary>
        public ScopeDisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="Scope"/>
        /// </summary>
        public ScopeDescription Description { get; set; }

        /// <summary>
        /// Gets or sets whether or not to emphasize the <see cref="Scope"/>
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// Gets or sets whether or not the <see cref="Scope"/> is required
        /// </summary>
        public bool Required { get; set; }
    }
}