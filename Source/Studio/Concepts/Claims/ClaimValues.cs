/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;

namespace Concepts.Claims
{
    /// <summary>
    /// Represents a collection of <see cref="ClaimValue"/>
    /// </summary>
    public class ClaimValues : IEnumerable<ClaimValue>
    {
        readonly IEnumerable<ClaimValue> _values;

        /// <summary>
        /// Initializes a new instance of <see cref="ClaimValues"/>
        /// </summary>
        /// <param name="values"></param>
        public ClaimValues(IEnumerable<ClaimValue> values) => _values = values;

        /// <inheritdoc/>
        public IEnumerator<ClaimValue> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}