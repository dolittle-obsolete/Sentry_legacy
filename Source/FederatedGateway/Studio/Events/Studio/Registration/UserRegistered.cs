/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Studio.Registration
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a user is registered
    /// </summary>
    public class UserRegistered : IEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UserRegistered"/>
        /// </summary>
        /// <param name="user">Unique identifier of the user</param>
        public UserRegistered(Guid user) => User = user;

        /// <summary>
        /// Gets the unique identifier for the user
        /// </summary>
        public Guid User { get; }
    }
}