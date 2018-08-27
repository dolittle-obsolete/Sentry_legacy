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
    public class Name : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> to <see cref="Name"/>
        /// </summary>
        /// <param name="name"><see cref="string"/> representation</param>
        public static implicit operator Name(string name) => new Name { Value = name };
    }
}