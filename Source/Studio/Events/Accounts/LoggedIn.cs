/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Accounts
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a user is logged in
    /// </summary>
    public class LoggedIn : IEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LoggedIn"/>
        /// </summary>
        /// <param name="user">User that logged in</param>
        public LoggedIn(Guid user)
        {
            User = user;
        }

        /// <summary>
        /// Gets the unique identifier representing the user
        /// </summary>
        public Guid User {  get; }
    }
    
}