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
    public class AuthContext
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

        /// <summary>
        /// Gets the current <see cref="Tenant"/>
        /// </summary>
        public Tenant Tenant { get; }

        /// <summary>
        /// Gets the current <see cref="Application"/>
        /// </summary>
        public Application Application { get; }
    }
}