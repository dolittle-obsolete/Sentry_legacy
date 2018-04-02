/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts
{
    /// <summary>
    /// Represents the concept of a name of an application
    /// </summary>
    public class ApplicationName : ConceptAs<string>
    {
        /// <summary>
        /// Implicitly convert from a <see cref="string"/> to an <see cref="ApplicationName"/>
        /// </summary>
        /// <param name="name"><see cref="string"/> to convert from</param>
        public static implicit operator ApplicationName(string name) => new ApplicationName { Value = name };
    }
}