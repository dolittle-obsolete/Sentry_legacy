/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Studio.Registration
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a user is associated with an authority
    /// </summary>
    public class UserAssociatedWithAuthority : IEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UserAssociatedWithAuthority"/>
        /// </summary>
        /// <param name="authority">Authority that will be associated</param>
        /// <param name="user">User that gets associated</param>
        /// <param name="identifier">Authority specific identifier</param>
        public UserAssociatedWithAuthority(
            Guid authority,
            Guid user,
            string identifier)
        {
            Authority = authority;
            User = user;
            Identifier = identifier;
        }

        /// <summary>
        /// Gets the unique identifier for the authority
        /// </summary>
        public Guid Authority { get; }

        /// <summary>
        /// Gets the unique identifier for the user
        /// </summary>
        public Guid User { get; }

        /// <summary>
        /// Gets the authority specific identifier for the user
        /// </summary>
        public string Identifier { get; }
    }
}