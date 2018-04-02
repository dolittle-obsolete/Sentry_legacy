/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Concepts
{
    /// <summary>
    /// Represents the concept of a unique identifier of a tenant
    /// </summary>
    public class TenantId : ConceptAs<Guid>
    {
        /// <summary>
        /// Implicitly convert from a <see cref="Guid"/> to a <see cref="TenantId"/>
        /// </summary>
        /// <param name="id"><see cref="Guid"/> to convert from</param>
        public static implicit operator TenantId(Guid id) => new TenantId { Value = id };
    }

}