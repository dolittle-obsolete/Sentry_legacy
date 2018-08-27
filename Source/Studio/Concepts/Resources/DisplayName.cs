/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts.Resources
{
    /// <summary>
    /// Represents the concept of a name for an identity resource
    /// </summary>
    public class DisplayName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> to <see cref="Name"/>
        /// </summary>
        /// <param name="displayName"><see cref="string"/> representation</param>
        public static implicit operator DisplayName(string displayName) => new DisplayName { Value = displayName };
    }
}