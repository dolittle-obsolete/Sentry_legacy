/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Read.Management;

namespace Infrastructure
{
    /// <summary>
    /// Represents the context used throughout
    /// </summary>
    public class AuthContext : IAuthContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AuthContext"/>
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="application"></param>
        public AuthContext(Tenant tenant, Application application)
        {
            Tenant = tenant;
            Application = application;
        }

        /// <inheritdoc/>
        public Tenant Tenant { get; }

        /// <inheritdoc/>
        public Application Application { get; }
    }
}