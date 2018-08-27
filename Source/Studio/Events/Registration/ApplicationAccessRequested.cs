/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Registration
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a user requests access to an application
    /// </summary>
    public class ApplicationAccessRequested : IEvent
    {
        /// <summary>
        /// Initializes a new instance of 
        /// </summary>
        /// <param name="tenant">Unique identifier of the tenant that owns the user requesting access</param>
        /// <param name="application">Unique identifier of the application</param>
        /// <param name="user">Unique identifier of the user</param>
        public ApplicationAccessRequested(Guid tenant, Guid application, Guid user)
        {
            Tenant = tenant;
            Application = application;
            User = user;
        }

        /// <summary>
        /// Gets the unique identifier for the tenant that owns the user requesting access
        /// </summary>
        public Guid Tenant { get; }

        /// <summary>
        /// Gets the unique identifier for the application
        /// </summary>
        /// <returns></returns>
        public Guid Application { get; }

        /// <summary>
        /// Gets the unique identifier for the user 
        /// </summary>
        public Guid User { get; }      
    }
}