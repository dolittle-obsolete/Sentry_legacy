/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Read.Infrastructure.Gateway.Applications;
using Read.Infrastructure.Gateway.Tenants;

namespace Read.Infrastructure.Gateway
{
    /// <summary>
    /// Defines the context used throughout
    /// </summary>
    public interface IAuthContext
    {
        /// <summary>
        /// Gets the current <see cref="Tenant"/>
        /// </summary>
        Tenant Tenant { get; }

        /// <summary>
        /// Gets the current <see cref="Application"/>
        /// </summary>
        Application Application { get; }
    }
}